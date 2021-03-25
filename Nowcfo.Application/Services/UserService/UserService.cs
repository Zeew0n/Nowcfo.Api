using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nowcfo.Application.Dtos.Email;
using Nowcfo.Application.Dtos.User.Request;
using Nowcfo.Application.Dtos.User.Response;
using Nowcfo.Application.Exceptions;
using Nowcfo.Application.Helper;
using Nowcfo.Application.IRepository;
using Nowcfo.Application.Services.CurrentUserService;
using Nowcfo.Domain.Models.AppUserModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Nowcfo.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public UserService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork,
            IConfiguration configuration, IMapper mapper
            , IApplicationDbContext context
            , ICurrentUserService currentUserService
            )
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
            _dbContext = context;
            _currentUserService = currentUserService;
        }

        #region Command Methods

        public async Task<IdentityResult> CreateAsync(AppUser appUser, string role)
        {
            try
            {
                string email = appUser.Email;
                string userName = appUser.Email;
                AppUser oldUserEmailDetail = await FindByEmailAsync(email);
                AppUser oldUserNameDetail = await FindByNameAsync(userName);
                if (oldUserEmailDetail != null)
                    throw new Exception($"DuplicateEmail: Email {email} is already taken.");
                if (oldUserNameDetail != null)
                    throw new Exception($"DuplicateUserName: UserName {userName} is already taken.");

                return await SaveUserAsync(appUser, role);
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw ;
            }
        }

        public async Task<UserLoginResponse> LoginAsync(AppUser appUser, string password)
        {
            try
            {
                var result = await _userManager.CheckPasswordAsync(appUser, password);
                if (!result)

                    return new UserLoginResponse
                    {
                        Message = "Invalid Password",
                        IsSuccess = false,
                    };

                //To Generate Token
                var token = GenerateJWTToken(appUser);
                string tokenValue = token.Result;
                return new UserLoginResponse
                {
                    Message = tokenValue,
                    IsSuccess = true

                };


                //return await LoginUserAsync(appUser);
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<IdentityResult> CreateUserAsync(AppUser appUser, string role)
        {
            try
            {
                string email = appUser.Email;
                string userName = appUser.UserName;
                AppUser oldUserEmailDetail = await FindByEmailAsync(email);
                AppUser oldUserNameDetail = await FindByNameAsync(userName);
                if (oldUserEmailDetail != null)
                    throw new Exception($"DuplicateEmail: Email {email} is already taken.");

                if (oldUserNameDetail != null)
                    throw new Exception($"DuplicateUserName: UserName {userName} is already taken.");

                return await SaveUserAsync(appUser, role);
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }


        public async Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token, string password)
        {
            try
            {
                string email = appUser.Email;
                string userName = appUser.UserName;
                AppUser oldUserEmailDetail = await FindByEmailAsync(email);
                AppUser oldUserUsername = await FindByUsernameAsync(userName);

                if (oldUserEmailDetail == null)
                    throw new Exception($"Email not available.");

                if (oldUserUsername == null)
                    throw new Exception($"Username not available.");

                return await ResetAsync(appUser, token, password);
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<IdentityResult> ResetAsync(AppUser appUser, string token, string password)
        {
            try
            {
                Guid id = appUser.Id;
                AppUser user = await FindByIdAsync(id);

                var userResult = await _userManager.ResetPasswordAsync(user, DecodeToken(token), password);
                return userResult;
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }


        //Confirm User Email and Reset Password

        public async Task<bool> ConfirmUserAsync(AppUser appUser, string token)
        {
            try
            {
                string email = appUser.Email;
                string userName = appUser.UserName;
                AppUser oldUserEmailDetail = await FindByEmailAsync(email);
                AppUser oldUserUsername = await FindByUsernameAsync(userName);

                if (oldUserEmailDetail == null)
                    throw new Exception($"Email not available.");

                if (oldUserUsername == null)
                    throw new Exception($"Username not available.");

                return await ConfirmAsync(appUser, token);
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw ;
            }
        }

        public async Task<bool> ConfirmAsync(AppUser appUser, string token)
        {
            try
            {
                Guid id = appUser.Id;
                AppUser user = await FindByIdAsync(id);
                var userResult = await _userManager.ConfirmEmailAsync(user, DecodeToken(token));
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw ;
            }
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }


        /// <summary>
        /// Creates a new user asynchronously.
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        private async Task<IdentityResult> SaveUserAsync(AppUser appUser, string role)
        {
            appUser.UserName = appUser.UserName.ToLower();
            appUser.IsAdmin = role.ToUpper() == DesignationAndRoleConstants.Admin;
            appUser.CreatedBy = _currentUserService.GetUser();
            appUser.CreatedDate = DateTime.Now;
            appUser.Password = CreatePassword(8);
            var userResult = await _userManager.CreateAsync(appUser,appUser.Password);
            return userResult;
        }

        public async Task<IdentityResult> UpdateUserAsync(AppUser oldUser, UpdateUserDto userRegiterDto, string role)
        {
            try
            {
                IdentityResult model = new IdentityResult();
                string email = userRegiterDto.Email;
                string userName = userRegiterDto.UserName;
                if (oldUser.Email == email)
                {
                    model = await EditUserAsync(oldUser, userRegiterDto, role);
                }
                else
                {
                    AppUser oldUserEmailDetail = await FindByEmailAsync(email);

                    if (oldUserEmailDetail != null)
                        throw new ApiException($"DuplicateEmail: Email {email} is already taken.");

                    model = await EditUserAsync(oldUser, userRegiterDto, role);
                }

                if (oldUser.UserName == userName)
                {
                    model = await EditUserAsync(oldUser, userRegiterDto, role);
                }
                else
                {
                    AppUser oldUserNameDetail = await FindByNameAsync(userName);

                    if (oldUserNameDetail != null)
                        throw new ApiException($"DuplicateEmail: UserName {userName} is already taken.");

                    model = await EditUserAsync(oldUser, userRegiterDto, role);
                }
                return model;
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        private async Task<IdentityResult> EditUserAsync(AppUser oldUser, UpdateUserDto userRegiterDto, string roleName)
        {
            oldUser.UserName = userRegiterDto.UserName.ToLower();
            oldUser.FirstName = userRegiterDto.FirstName.Trim();
            oldUser.LastName = userRegiterDto.LastName.Trim();
            oldUser.Email = userRegiterDto.Email.Trim();
            oldUser.PhoneNumber = userRegiterDto.PhoneNumber;
            oldUser.UpdatedBy = _currentUserService.GetUser();
            oldUser.UpdatedDate = DateTime.Now;
            oldUser.IsAdmin = roleName.ToUpper() == DesignationAndRoleConstants.Admin;
            var userResult = await _userManager.UpdateAsync(oldUser);
            return userResult;
        }

        public async Task<IdentityResult> SignUpUserAsync(AppUser oldUser, SignUpUserDto signUpUserDto)
        {
            try
            {

                var result = await _userManager.UpdateSecurityStampAsync(oldUser);
                oldUser.UserName = signUpUserDto.UserName.Trim();
                oldUser.EmailConfirmed = true;
                oldUser.PasswordHash = _userManager.PasswordHasher.HashPassword(oldUser, signUpUserDto.Password);
                var userResult = await _userManager.UpdateAsync(oldUser);
                return userResult;
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(AppUser userDetail, Guid id)
        {
            try
            {
                if (userDetail == null)
                {
                    Log.Error("Error: User with {Id} doesn't exist.", id);
                    throw new ApiException("Provided user doesn't exists.");
                }

                userDetail.UpdatedBy = _currentUserService.GetUser();
                userDetail.UpdatedDate = DateTime.Now;
                var result = await _userManager.DeleteAsync(userDetail);
                if (result.Succeeded) return true;
                return false;
                
            }

            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }


        #endregion Command Methods

        #region GetMethods

        /// <summary>
        /// Verify password asynchronously.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> CheckPasswordAsync(AppUser user, string password) =>
            await _userManager.CheckPasswordAsync(user, password);

        /// <summary>
        /// Gets user data by name asynchronously.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<AppUser> FindByNameAsync(string name)
        {
            
            return await _dbContext.AppUsers.Where(x => x.UserName == name).FirstOrDefaultAsync();
           
        }

        /// <summary>
        /// Gets user data by email asynchronously.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<AppUser> FindByEmailAsync(string email)
        {
            
            return await _dbContext.AppUsers.Where(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<AppUser> FindByUsernameAsync(string username)
        {
            return await _dbContext.AppUsers.Where(x => x.UserName == username).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Gets user data by id asynchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AppUser> FindByIdAsync(Guid id) => await _userManager.FindByIdAsync(id.ToString());

        public async Task<IReadOnlyCollection<UserListDto>> GetAllUsers()
        {
            var result = await (from user in _dbContext.AppUsers
                                join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                                join role in _dbContext.AppRoles on userRole.RoleId equals role.Id
                                select new
                                {
                                    user.Id,
                                    user.UserName,
                                    user.Email,
                                    user.FirstName,
                                    user.LastName,
                                    user.CreatedDate,
                                    Role = role.Name,
                                    RoleId= role.Id,
                                    user.PhoneNumber,
                                    user.Address,
                                    user.City,
                                    user.ZipCode,
                                    EmailConfirmmed = user.EmailConfirmed,
                                    IsDeleted = EF.Property<bool>(user, "IsDeleted")
                                }).Where(x => !x.IsDeleted).OrderByDescending(q => q.CreatedDate).ToListAsync();
            return result.Select(user => new UserListDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber,
                Address=user.Address,
                City=user.City,
                ZipCode=user.ZipCode,
                RoleId = user.RoleId.ToString(),
                Status = user.IsDeleted ? UserConstants.Deleted : (user.EmailConfirmmed ? UserConstants.Active : UserConstants.InActive)
            }).ToList();
        }

        public async Task<IReadOnlyCollection<UserListDto>> GetAllAdmins()
        {
            var result = await (from user in _dbContext.AppUsers
                                join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                                join role in _dbContext.AppRoles on userRole.RoleId equals role.Id
                                select new
                                {
                                    user.Id,
                                    user.UserName,
                                    user.Email,
                                    user.FirstName,
                                    user.LastName,
                                    user.CreatedDate,
                                    Role = role.Name,
                                    user.PhoneNumber,
                                    EmailConfirmmed = user.EmailConfirmed,
                                    IsDeleted = EF.Property<bool>(user, "IsDeleted")
                                }).Where(x => !x.IsDeleted).OrderByDescending(q => q.CreatedDate).ToListAsync();
            return result.Select(user => new UserListDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber,
                Status = user.IsDeleted ? UserConstants.Deleted : (user.EmailConfirmmed ? UserConstants.Active : UserConstants.InActive)
            }).ToList();
        }


        public async Task<UserDetailsDto> GetUserDetailById(Guid userId)
        {
            return await (from user in _dbContext.AppUsers
                          join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                          join role in _dbContext.AppRoles on userRole.RoleId equals role.Id
                          where user.Id == userId
                          select new UserDetailsDto
                          {
                              Id = user.Id,
                              UserName = user.UserName,
                              Email = user.Email,
                              FirstName = user.FirstName,
                              LastName = user.LastName,
                              PhoneNumber = user.PhoneNumber,
                              RoleId = role.Id,
                              Role = role.Name
                          }).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Creates encoded email token with redirect uri.
        /// </summary>
        /// <param name="appUser"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<EmailDto> GetEmailTokenWithContentAsync(AppUser appUser)
        {
            try
            {
                //Invalidates any previous codes sent to the user
                await _unitOfWork.SaveChangesAsync();
                var result = await _userManager.UpdateSecurityStampAsync(appUser);
                Uri callBackUri = await SetClientURIWithEncodedToken(appUser, SharedConstants.URIParam);
                var emailMessage = new EmailDto
                {
                    To = appUser.Email,
                    Subject = SharedConstants.SignUpSubject,
                    Body = @$"Your NowCFO Org account has been created. Your default password is <b> {appUser.Password}</b>. Click the link below to confirm your email address and finish the sign up process.This link will expire after 24 hours.<br/>
                              <a href={callBackUri}>Click Here</a> "
                };
                return emailMessage;
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<EmailDto> GetEmailTokenWithContentResetAsync(AppUser appUser)
        {
            try
            {
                //Invalidates any previous codes sent to the user
                await _unitOfWork.SaveChangesAsync();
                var result = await _userManager.UpdateSecurityStampAsync(appUser);
                Uri callBackUri = await SetClientURIWithEncodedTokenResetPassword(appUser, SharedConstants.URIParamReset);
                var emailMessage = new EmailDto
                {
                    To = appUser.Email,
                    Subject = SharedConstants.ResetSubject,
                    Body = @$"Dear user, Click the link below to reset your password. This link will expire after 24 hours.<br/>
                              <a href={callBackUri}>Click Here</a>"
                };
                return emailMessage;
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }
        /// <summary>
        /// Encodes email token and generates a client URI.
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        public async Task<Uri> SetClientURIWithEncodedToken(AppUser appUser, string uriParam)
        {
            string token = await EncodeTokenAsync(appUser);
            string clientUri = _configuration["URL"];
            appUser.UserName = appUser.UserName ?? "0";
            string generatedUri = $"{clientUri}/{uriParam}/{appUser.Id}/{appUser.Email}/{token}";
            Uri callBackUri = new Uri(Uri.EscapeUriString(generatedUri));
            return callBackUri;
        }

        public async Task<Uri> SetClientURIWithEncodedTokenResetPassword(AppUser appUser, string uriParam)
        {
            string token = await EncodePasswordTokenAsync(appUser);
            string clientUri = _configuration["URL"];
            appUser.UserName = appUser.UserName ?? "0";
            string generatedUri = $"{clientUri}/{uriParam}/{appUser.Id}/{appUser.Email}/{appUser.UserName}/{token}";
            Uri callBackUri = new Uri(Uri.EscapeUriString(generatedUri));
            return callBackUri;
        }

        //Generate Token
        public async Task<string> GenerateJWTToken(AppUser appUser)
        {
            var user = await _userManager.FindByEmailAsync(appUser.Email);
            var claims = new[]
            {
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenAsString;
        }


        private async Task<string> EncodeTokenAsync(AppUser appUser)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            byte[] tokenBytes = System.Text.Encoding.UTF8.GetBytes(token);
            return Convert.ToBase64String(tokenBytes);
        }

        /// <summary>
        /// Generates token for password reset and encodes into base64.
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        private async Task<string> EncodePasswordTokenAsync(AppUser appUser)
        {
            string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            byte[] tokenBytes = System.Text.Encoding.UTF8.GetBytes(token);
            return Convert.ToBase64String(tokenBytes);
        }


        /// <summary>
        /// Validates if provided email token is valid or not
        /// </summary>
        /// <param name="appUser"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> ValidateEmailTokenAsync(AppUser appUser)
        {

            try
            {
                return await _userManager.IsEmailConfirmedAsync(appUser);
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }




        /// <summary>
        /// Generates token for email and encodes into base64.
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        private string DecodeToken(string token)
        {
            byte[] data = Convert.FromBase64String(token);
            return System.Text.Encoding.UTF8.GetString(data);
        }

        #endregion GetMethods
    }
}