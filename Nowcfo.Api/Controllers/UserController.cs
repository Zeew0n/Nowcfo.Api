﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nowcfo.API.Attributes;
using Nowcfo.API.Controllers.Base;
using Nowcfo.Application.Dtos.Email;
using Nowcfo.Application.Dtos.User.RefreshToken;
using Nowcfo.Application.Dtos.User.Request;
using Nowcfo.Application.Extensions;
using Nowcfo.Application.Helper;
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

    


        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequestDto request)
        {


            try
            {
                var result = await _authService.AuthenticateAsync(request);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }




        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto tokenRequestDto)
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(tokenRequestDto.RefreshToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPost("revoke-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequestDto model)
        {
            try
            {
                await _authService.RevokeTokenAsync(model.Token);
                return Ok(new { message = "Token revoked" });
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }




        #region Commands

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] CreateUserDto userRegisterDto)
        {
            try
            {
                AppUser appUser = await _userService.FindByEmailAsync(userRegisterDto.UserName);

                if (appUser == null)
                {
                    return BadRequest(HandleActionResult($"User Registered with this email Not Found!", StatusCodes.Status400BadRequest));
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
        [AllowAnonymous]
        public async Task<IActionResult> UpdatePasswordReset([FromBody] UpdateUserDto userRegisterDto)
        {
            try
            {
                Guard.Against.InvalidPasswordCompare(userRegisterDto.Password, userRegisterDto.ConfirmPassword, nameof(userRegisterDto.Password), nameof(userRegisterDto.ConfirmPassword));
                AppUser User = await _userService.FindByIdAsync(userRegisterDto.Id);
                var result = await _userService.ResetPasswordAsync(User,userRegisterDto.token,userRegisterDto.Password);
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

        [HttpPut("changepassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
        {
            try
            {
                var appuser = await _userManager.FindByNameAsync(updatePasswordDto.UserName);
                var x = await _userManager.ChangePasswordAsync(appuser, updatePasswordDto.CurrentPassword, updatePasswordDto.Password);
                if (x.Succeeded)
                {
                    var userId = appuser.Id;
                    var user = await _userService.FindByIdAsync(userId);
                    return Ok(user);
                }
                return BadRequest(HandleActionResult($"Password update failed.", StatusCodes.Status400BadRequest));

            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpGet]
        [Route("confirmUser")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmUser(Guid userId, string token)
        {
            try
            {
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
        [Permission(CrudPermission.AddUser)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userRegisterDto)
        {
            try
            {
                Guid roleId = userRegisterDto.RoleId;
                Guard.Against.InvalidEmail(userRegisterDto.Email, nameof(userRegisterDto.Email));
                Guard.Against.InvalidPhone(userRegisterDto.PhoneNumber);
                string role = await _roleServices.GetRoleNameByIdAsync(roleId);
                AppUser appUser = _mapper.Map<CreateUserDto, AppUser>(userRegisterDto);
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
        [Permission(CrudPermission.UpdateUser)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto userRegisterDto)
        {
            try
            {
                string roleName = await _roleServices.GetRoleNameByIdAsync(userRegisterDto.RoleId);
                var appuser = await _userManager.FindByIdAsync(userRegisterDto.Id.ToString());
                var appuserIsAdmin = roleName.ToLower() == "superadmin";
                appuser.IsAdmin = appuserIsAdmin;
                var x = await _userManager.UpdateAsync(_mapper.Map(userRegisterDto, appuser));
                if (x.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(appuser);
                    var removeResult = await _userManager.RemoveFromRolesAsync(appuser, roles);
                    if (removeResult.Succeeded)
                    {
                        var userId = appuser.Id;
                        var user = await _userService.FindByIdAsync(userId);
                        var roleResult = await _roleServices.AddToRoleAsync(user, roleName);
                        return Ok(user);
                    }
                }
                return BadRequest(HandleActionResult($"User update failed.", StatusCodes.Status400BadRequest));

            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpDelete("delete/{userId}")]
        [Permission(CrudPermission.DeleteUser)]
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


        [HttpGet("CheckIfUsernameExists/{userName}")]
        public async Task<IActionResult> CheckIfUsernameExists(string userName)
        {
            try
            {
                var checkValue = await _userService.FindByNameAsync(userName);
                if (checkValue!=null)
                    return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }


        [HttpGet("CheckIfEmailExists/{email}")]
        public async Task<IActionResult> CheckIfEmailExists(string email)
        {
            try
            {
                var checkValue = await _userService.FindByEmailAsync(email);
                if (checkValue!=null)
                    return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }




        #endregion Commands

        #region Queries

        [HttpGet("listallusers")]
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

        


        /// <summary>
        /// Verify email token.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("verifyEmailToken")]
        [AllowAnonymous]

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