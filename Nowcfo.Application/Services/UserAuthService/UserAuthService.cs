using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nowcfo.Application.Dtos.User.Request;
using Nowcfo.Application.Dtos.User.Response;
using Nowcfo.Application.Extensions;
using Nowcfo.Application.IRepository;
using Nowcfo.Application.Services.JwtService;
using Nowcfo.Application.Services.UserService;
using Nowcfo.Domain.Models.AppUserModels;
using Serilog;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Nowcfo.Application.Services.UserAuthService
{
    public class UserAuthService : IUserAuthService
    {
        
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IApplicationDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public UserAuthService
        (IUnitOfWork unitOfWork
            ,IApplicationDbContext dbContext
            ,UserManager<AppUser> userManager
            ,IJwtService jwtService
            ,IUserService userService
            ,RoleManager<AppRole> roleManager
            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        #region AuthToken

        public async Task<AuthenticationResponseDto> AuthenticateAsync(AuthenticationRequestDto request)
        {
            try
            {
                
                var claimsIdentity = await GetClaimsIdentityAsync(request.UserName, request.Password);
                var jwtResponse = await _jwtService.GenerateJwt(claimsIdentity);
                await GenerateRefreshToken(claimsIdentity);
                jwtResponse.RefreshToken = claimsIdentity.RefreshToken.Token;
                jwtResponse.RefreshTokenExpiry = claimsIdentity.RefreshToken.ExpiryDate;
                return jwtResponse;
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }


        private async Task GenerateRefreshToken(AppUserDto userDetails)
        {
            var currentRefreshToken = userDetails.RefreshToken;
            if (currentRefreshToken != null)
                userDetails.RefreshToken = currentRefreshToken;
            else
            {
                Guid userId = userDetails.Id;
                var refreshToken = CreateRefreshToken(userId);
                var refreshModel = refreshToken.MapToRefreshTokenResponseDTO();
                refreshToken.CreatedBy = userId;
                _dbContext.RefreshTokens.Add(refreshToken);
                _dbContext.SaveChange();
                userDetails.RefreshToken = refreshModel;
            }
        }

        private RefreshToken CreateRefreshToken(Guid userId)
        {
            var randomNumber = new byte[32];
            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(randomNumber);
                return RefreshToken.CreateRefreshToken(Convert.ToBase64String(randomNumber), userId);
            }
        }

        /// <summary>
        /// Verifies user credentials and returns claim list asynchronously.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<AppUserDto> GetClaimsIdentityAsync(string userName, string password)
        {
            try
            {
                AppUser appUser = await VerifyEmailOrUserNameAsync(userName);
                if (appUser == null)
                    throw new Exception($"No Accounts Registered with {userName}.");

                if (await _userManager.IsEmailConfirmedAsync(appUser))
                {
                    var userDetails = await GetUserDetailsByUserId(appUser.Id);
                    if (userDetails == null)
                        throw new Exception($"You must be assigned a role before you login.");

                    return await VerifyUserNamePasswordAsync(password, appUser, userDetails);
                }

                throw new Exception("You must confirm your email before you log in.");
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Verifies username and password and returns claim list asynchronously.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="userToVerify"></param>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        private async Task<AppUserDto> VerifyUserNamePasswordAsync(string password, AppUser userToVerify, AppUserDto userDetails)
        {
            if (await _userService.CheckPasswordAsync(userToVerify, password))
            {
                var claimDto = new ClaimDto
                {
                    Id = userToVerify.Id,
                    UserName = userDetails.UserName,
                    Email = userToVerify.Email,
                };
                userDetails.ClaimsIdentity = await Task.FromResult(_jwtService.GenerateClaimsIdentity(claimDto));
                return userDetails;
            }

            throw new Exception("Invalid email or password.");
        }

        private async Task<AppUserDto> GetUserDetailsByUserId(Guid userId)
        {
            
            var userDetails = await (from user in _dbContext.AppUsers
                                     join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                                     join role in _dbContext.AppRoles on userRole.RoleId equals role.Id
                                     join refToken in _dbContext.RefreshTokens on user.Id equals refToken.UserId into rt
                                     from refreshToken in rt.DefaultIfEmpty()
                                     where user.Id == userId
                                     select new
                                     {
                                         user.Id,
                                         FullName = user.FirstName + " " + user.LastName,
                                         user.UserName,
                                         user.Email,
                                         user.IsAdmin,
                                         RoleId = role.Id,
                                         RoleName = role.Name,
                                         RefreshToken = refreshToken
                                     }).ToListAsync();
            return userDetails.GroupBy(t => t.RoleId)
                 .Select(q =>
                 {
                     var refreshToken = q.Select(t => t.RefreshToken).FirstOrDefault();
                     return new AppUserDto
                     {
                         Id = q.Select(t => t.Id).FirstOrDefault(),
                         FullName = q.Select(t => t.FullName).FirstOrDefault(),
                         UserName = q.Select(t => t.UserName).FirstOrDefault(),
                         Email = q.Select(t => t.Email).FirstOrDefault(),
                         IsAdmin = q.Select(t => t.IsAdmin).FirstOrDefault(),
                         RoleId = q.Key,
                         RoleName = q.Select(t => t.RoleName).FirstOrDefault(),
                         RefreshToken = refreshToken?.MapToRefreshTokenResponseDTO()
                     };
                 }).FirstOrDefault();
        }

        /// <summary>
        /// Verifies provided user data is username or email and fetches data asynchronously.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userToVerify"></param>
        /// <returns></returns>
        private async Task<AppUser> VerifyEmailOrUserNameAsync(string userName)
        {
            return IsEmail(userName) ? await _userService.FindByEmailAsync(userName) : await _userService.FindByNameAsync(userName);
        }

        /// <summary>
        /// Checks username or email.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private bool IsEmail(string userName) => new EmailAddressAttribute().IsValid(userName);

        #endregion AuthToken

        #region RefreshToken

        public async Task<AuthenticationResponseDto> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var claimsIdentity = await GetClaimsIdentityAsync(refreshToken);
                var jwtResponse = await _jwtService.GenerateJwt(claimsIdentity);
                await UpdateRefreshToken(claimsIdentity);
                jwtResponse.RefreshToken = claimsIdentity.RefreshToken.Token;
                jwtResponse.RefreshTokenExpiry = claimsIdentity.RefreshToken.ExpiryDate;
                return jwtResponse;
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        private async Task UpdateRefreshToken(AppUserDto userDetails)
        {
            Guid userId = userDetails.Id;
            var currentRefreshToken = _dbContext.RefreshTokens.Where(m => m.UserId == userId).FirstOrDefault();
            currentRefreshToken.UpdateToken(GenerateRefreshToken());
            currentRefreshToken.UpdatedBy = userId;
            var refreshModel = currentRefreshToken.MapToRefreshTokenResponseDTO();
            _dbContext.RefreshTokens.Update(currentRefreshToken);
            _dbContext.SaveChange();
            userDetails.RefreshToken = refreshModel;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var generator = new RNGCryptoServiceProvider())
            {
                generator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// Get claim list asynchronously.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<AppUserDto> GetClaimsIdentityAsync(string token)
        {
            try
            {
                var userDetails = await GetUserDetailsByRefreshToken(token);
                if (userDetails == null)
                    throw new Exception($"Token did not match any users.");

                var refreshToken = userDetails.RefreshToken;
                return await GenerateClaimsIdentity(userDetails);
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Returns claim list asynchronously.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="userToVerify"></param>
        /// <param name="userDetails"></param>
        /// <returns></returns>
        private async Task<AppUserDto> GenerateClaimsIdentity(AppUserDto userDetails)
        {
            var claimDTO = new ClaimDto
            {
                Id = userDetails.Id,
                UserName = userDetails.UserName,
                FullName = userDetails.FullName,
                Email = userDetails.Email,
                RoleId = userDetails.RoleId,
                Role = userDetails.RoleName,

            };
            userDetails.ClaimsIdentity = await Task.FromResult(_jwtService.GenerateClaimsIdentity(claimDTO));
            return userDetails;
        }

        private async Task<AppUserDto> GetUserDetailsByRefreshToken(string token)
        {

            var userDetails = await (from refreshToken in _dbContext.RefreshTokens
                                     join user in _dbContext.AppUsers on refreshToken.UserId equals user.Id
                                     join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                                     join role in _dbContext.AppRoles on userRole.RoleId equals role.Id
                                     where refreshToken.Token == token
                                     select new
                                     {
                                         user.Id,
                                         FullName = user.FirstName + " " + user.LastName,
                                         user.UserName,
                                         user.Email,
                                         user.IsAdmin,
                                         RoleId = role.Id,
                                         RoleName = role.Name,
                                         RefreshToken = refreshToken
                                     }).ToListAsync();
            return userDetails.GroupBy(t => t.RoleId)
                 .Select(q =>
                 {
                     var refreshToken = q.Select(t => t.RefreshToken).FirstOrDefault();
                     return new AppUserDto
                     {
                         Id = q.Select(t => t.Id).FirstOrDefault(),
                         FullName = q.Select(t => t.FullName).FirstOrDefault(),
                         UserName = q.Select(t => t.UserName).FirstOrDefault(),
                         Email = q.Select(t => t.Email).FirstOrDefault(),
                         IsAdmin = q.Select(t => t.IsAdmin).FirstOrDefault(),
                         RoleId = q.Key,
                         RoleName = q.Select(t => t.RoleName).FirstOrDefault(),
                         RefreshToken = refreshToken.MapToRefreshTokenResponseDTO()
                     };
                 }).FirstOrDefault();
        }

        #endregion RefreshToken

        #region RevokeRefreshToken

        public async Task<bool> RevokeTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new Exception($"Token is required.");

            var currentRefreshToken = await _dbContext.RefreshTokens.Where(q => q.Token == token).FirstOrDefaultAsync();

            // return false if no user found with token
            if (currentRefreshToken == null)
                throw new Exception($"Token did not match any users.");

            _dbContext.RefreshTokens.Remove(currentRefreshToken);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        #endregion RevokeRefreshToken
    }
}