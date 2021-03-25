using Microsoft.AspNetCore.Identity;
using Nowcfo.Application.Dtos.Email;
using Nowcfo.Application.Dtos.User.Request;
using Nowcfo.Application.Dtos.User.Response;
using Nowcfo.Domain.Models.AppUserModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.Services.UserService
{
    public interface IUserService
    {
        Task<IdentityResult> CreateAsync(AppUser appUser, string role);
        Task<UserLoginResponse> LoginAsync(AppUser appUser, string password);
       // Task<UserLoginResponse> LoginUserAsync(AppUser appUser);

        Task<IdentityResult> CreateUserAsync(AppUser appUser, string role);

        //Task<IdentityResult> UpdateUserAsync(AppUser oldUser, UpdateUserDto userRegiterDto);

        //Task<IdentityResult> SignUpUserAsync(SignUpUserDto signUpUserDto);

        Task<IdentityResult> UpdateUserAsync(AppUser oldUser, UpdateUserDto userRegisterDto, string role);

        Task<IdentityResult> SignUpUserAsync(AppUser oldUser, SignUpUserDto signUpUserDto);

        Task<bool> DeleteUserAsync(AppUser userDetail, Guid id);

        Task<IReadOnlyCollection<UserListDto>> GetAllUsers();

        Task<IReadOnlyCollection<UserListDto>> GetAllAdmins();

        Task<UserDetailsDto> GetUserDetailById(Guid userId);

        Task<AppUser> FindByEmailAsync(string email);

        Task<AppUser> FindByIdAsync(Guid id);

        Task<AppUser> FindByNameAsync(string name);

        Task<bool> CheckPasswordAsync(AppUser user, string password);

        Task<EmailDto> GetEmailTokenWithContentAsync(AppUser appUser);

        //Task<bool> ValidateEmailTokenAsync(AppUser appUser, string token);

        Task<bool> ValidateEmailTokenAsync(AppUser appUser);


        Task<IdentityResult> ResetPasswordAsync(AppUser appUser, string token,string password);
        Task<IdentityResult> ResetAsync(AppUser appUser, string token,string password);
        Task<EmailDto> GetEmailTokenWithContentResetAsync(AppUser appUser);

        Task<string> GenerateJWTToken(AppUser user);



        Task<bool> ConfirmUserAsync(AppUser appUser, string token);

    }
}