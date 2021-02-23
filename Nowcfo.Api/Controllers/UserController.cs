using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nowcfo.Application.Dtos.Email;
using Nowcfo.Application.Dtos.User.Request;
using Nowcfo.Application.Extensions;
using Nowcfo.Application.IRepository;
using Nowcfo.Application.Services.EmailService;
using Nowcfo.Application.Services.RoleService;
using Nowcfo.Application.Services.UserAuthService;
using Nowcfo.Application.Services.UserService;
using Nowcfo.Domain.Models.AppUserModels;
using Nowcfo.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace Nowcfo.API.Controllers
{
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly IUserAuthService _authService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleServices;
        private readonly IMailService _emailService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public UserController(IUserAuthService authService, IUserService userService,
             IRoleService roleServices, IMapper mapper,IMailService emailService, ApplicationDbContext context, IUnitOfWork unitOfWork,UserManager<AppUser> userManager)
        {
            _authService = authService;
            _userService = userService;
            _mapper = mapper;
            _roleServices = roleServices;
            _emailService = emailService;
            _context = context;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        #region Registration

        [HttpPost("authenticatetenant")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateTenantAsync([FromBody] AuthenticationRequestDto request)
        {
            try
            {
                AppUser appUser = await _userService.FindByEmailAsync(request.UserName);
                if (appUser == null)
                {
                    return BadRequest(HandleActionResult($"User Not Found!", StatusCodes.Status400BadRequest));

                }
                //if(appUser.TenantInformationTenantId!=request.TenantId)
                //{
                //    return BadRequest(HandleActionResult($"Tenant User Not Found!", StatusCodes.Status400BadRequest));

                //}
                var result = await _authService.AuthenticateTenantAsync(request);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult($"Username or Password is Invalid!", StatusCodes.Status400BadRequest));
            }
        }


        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequestDto request)
        {


            try
            {
                var result = await _authService.AuthenticateAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult($"Username or Password is Invalid!", StatusCodes.Status400BadRequest));
            }
        }


        #region Commands

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] CreateUserDto userRegisterDTO)
        {
            try
            {
                AppUser appUser = await _userService.FindByEmailAsync(userRegisterDTO.UserName);

                if (appUser == null)
                {
                    return BadRequest(HandleActionResult($"Email Not Found!", StatusCodes.Status400BadRequest));
                }

                Guid appuserid = Guid.Parse(appUser.Id.ToString());
                AppUser appnewUser = await _userService.FindByIdAsync(appuserid);
                if (appnewUser != null)
                {
                    var emailBody = await _userService.GetEmailTokenWithContentResetAsync(appnewUser);
                    var emailCommand = new EmailDto
                    {
                        To = emailBody.To,
                        Body = emailBody.Body,
                        Subject = emailBody.Subject
                    };
                    await _emailService.SendEmailAsync(emailCommand);
                    return Ok();
                }

                return BadRequest(HandleActionResult($"Email Not Found!", StatusCodes.Status400BadRequest));
            }

            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPut("updatepassword")]
        public async Task<IActionResult> UpdatePasswordReset([FromBody] UpdateUserDto userRegisterDTO)
        {
            try
            {
                Guard.Against.InvalidPasswordCompare(userRegisterDTO.Password, userRegisterDTO.ConfirmPassword, nameof(userRegisterDTO.Password), nameof(userRegisterDTO.ConfirmPassword));
                AppUser User = await _userService.FindByIdAsync(userRegisterDTO.Id);
                var result = await _userService.ResetPasswordAsync(User,userRegisterDTO.token,userRegisterDTO.Password);
                if (result.Succeeded)
                {
                    return Ok();

                }
                return BadRequest(HandleActionResult($"Password Reset failed.", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }



        [HttpGet]
        [Route("confirmUser")]
        public async Task<IActionResult> ConfirmUser(Guid userId, string token)
        {
            try
            {
               // Guard.Against.InvalidPasswordCompare(userRegisterDTO.Password, userRegisterDTO.ConfirmPassword, nameof(userRegisterDTO.Password), nameof(userRegisterDTO.ConfirmPassword));
                AppUser User = await _userService.FindByIdAsync(userId);
                var result = await _userService.ConfirmUserAsync(User, token);
                if (result)
                {
                    return Ok();

                }
                return BadRequest(HandleActionResult($"Confirm User Failed.", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }



        [HttpPost("create")]
       // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userRegisterDTO)
        {
            try
            {
                Guid roleId = userRegisterDTO.RoleId;
                //Guard.Against.NullOrEmpty(userRegisterDTO.FirstName, nameof(userRegisterDTO.FirstName));
                //Guard.Against.NullOrEmpty(userRegisterDTO.LastName, nameof(userRegisterDTO.LastName));
                Guard.Against.InvalidEmail(userRegisterDTO.Email, nameof(userRegisterDTO.Email));
                //Guard.Against.InvalidEmail(userRegisterDTO.ConfirmEmail, nameof(userRegisterDTO.ConfirmEmail));
                //Guard.Against.InvalidCompare(userRegisterDTO.Email, userRegisterDTO.ConfirmEmail, nameof(userRegisterDTO.Email), nameof(userRegisterDTO.ConfirmEmail));
                //Guard.Against.NullOrEmpty(roleId, nameof(roleId));
                Guard.Against.InvalidPhone(userRegisterDTO.PhoneNumber);
                string role = await _roleServices.GetRoleNameByIdAsync(roleId);
                //default role removed for test purpose
                AppUser appUser = _mapper.Map<CreateUserDto, AppUser>(userRegisterDTO);
               

                

                var result = await _userService.CreateAsync(appUser, role);
                if (result.Succeeded)
                {

                    var id1 = appUser.Id;
                    var user = await _userService.FindByIdAsync(appUser.Id);

                    var roleResult = await _roleServices.AddToRoleAsync(user,role );
                    if (roleResult.Succeeded)
                    {
                        var emailBody = await _userService.GetEmailTokenWithContentAsync(user);
                        var emailCommand = new EmailDto
                        {
                            To = emailBody.To,
                            Body = emailBody.Body,
                            Subject = emailBody.Subject
                        };
                        await _emailService.SendEmailAsync(emailCommand);
                        return Ok(user);
                    }

                    return BadRequest(HandleActionResult($"User registration failed.", StatusCodes.Status400BadRequest));
                }

                string identityErrors = string.Empty;
                foreach (var item in result.Errors)
                {
                    identityErrors = string.Concat(identityErrors, item.Code, ": ", item.Description);
                }
                return BadRequest(HandleActionResult($"User registration failed. { identityErrors }", StatusCodes.Status400BadRequest));
            }
            
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPut("update")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto userRegisterDTO)
        {
            try
            {
                var appuser = await _userManager.FindByIdAsync(userRegisterDTO.Id.ToString());

                var x = await _userManager.UpdateAsync(_mapper.Map(userRegisterDTO, appuser));


                if (x.Succeeded)
                {

                    var roles = await _userManager.GetRolesAsync(appuser);
                    var removeresult = await _userManager.RemoveFromRolesAsync(appuser, roles);
                    if (removeresult.Succeeded)
                    {
                        var userId = appuser.Id;
                        var user = await _userService.FindByIdAsync(userId);
                        string roleName = await _roleServices.GetRoleNameByIdAsync(userRegisterDTO.RoleId);
                        var roleResult = await _roleServices.AddToRoleAsync(user, roleName);
                        return Ok(user);

                    }
                   return BadRequest(HandleActionResult($"User registration failed.", StatusCodes.Status400BadRequest));
                }
                return BadRequest(HandleActionResult($"User registration failed.", StatusCodes.Status400BadRequest));

            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpDelete("delete/{userId}")]
      //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                Guid userIdd = Guid.Parse(userId);
                AppUser oldUser = await _userService.FindByIdAsync(userIdd);
                bool result = await _userService.DeleteUserAsync(oldUser, userIdd);
                if (result)
                {
                    return NoContent();
                }

                return BadRequest(HandleActionResult($"User delete failed.", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPost("signup")]
        //[AllowAnonymous]
        //[Authorize]

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> SignUpUser([FromBody] SignUpUserDto userRegisterDTO)
        {
            try
            {
                Guard.Against.NullOrEmpty(userRegisterDTO.UserName, nameof(userRegisterDTO.UserName));
                Guard.Against.InvalidPasswordCompare(userRegisterDTO.Password, userRegisterDTO.ConfirmPassword, nameof(userRegisterDTO.Password), nameof(userRegisterDTO.ConfirmPassword));
                var result = await _userService.SignUpUserAsync(userRegisterDTO);
                if (result.Succeeded)
                    return Ok();

                string identityErrors = string.Empty;
                foreach (var item in result.Errors)
                {
                    identityErrors = string.Concat(identityErrors, item.Code, ": ", item.Description);
                }
                return BadRequest(HandleActionResult($"User registration failed. { identityErrors }", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] CreateUserDto userRegisterDTO)
        {
            try
            {
                var user= await _userService.FindByNameAsync(userRegisterDTO.UserName);
                var result = await _userService.LoginAsync(user,userRegisterDTO.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }


        #endregion Commands

        #region Queries

        [HttpGet("listallusers")]
        //[Authorize]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }


        #endregion Commands

        #region Queries

        [HttpGet("listalladmins")]
        //[Authorize]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllAdmins()
        {
            try
            {
                var result = await _userService.GetAllAdmins();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }


        /// <summary>
        /// Verify email token.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("verifyEmailToken")]
        public async Task<IActionResult> VerifyEmailToken(Guid userId, string token)
        {
            try
            {
                AppUser appUser = await _userService.FindByIdAsync(userId);
                //token = token.Replace(" ", "%2b");
                var tokenResult = await _userService.ValidateEmailTokenAsync(appUser);
                if (tokenResult)
                    return Ok();

                return BadRequest(HandleActionResult("Invalid token", StatusCodes.Status400BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult($"Token validation failed. { ex.Message}", StatusCodes.Status400BadRequest));
            }
        }


        [HttpGet("get/{userId}")]
       // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserDetailById(Guid userId)
        {
            try
            {
                var result = await _userService.GetUserDetailById(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        #endregion Queries
    }
}

#endregion Registration