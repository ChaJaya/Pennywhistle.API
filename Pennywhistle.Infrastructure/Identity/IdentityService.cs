using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pennywhistle.Application;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.Common.Models;
using Pennywhistle.Application.Common.Variables;
using Pennywhistle.Domain.Common;
using Pennywhistle.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace Pennywhistle.Infrastructure.Identity
{
    /// <summary>
    /// Implements identity service
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            try
            {
                var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

                return user.UserName;
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                throw;
            }
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(Register register)
        {
            var user = new ApplicationUser
            {
                UserName = register.UserName,
                Email = register.Email,
                PasswordHash = register.Password

            };

            try
            {
                var result = await _userManager.CreateAsync(user, register.Password);

                var role = await _roleManager.FindByNameAsync(register.Role);
                result = await _userManager.AddToRoleAsync(user, role.NormalizedName);

                return (result.ToApplicationResult(), user.Id);
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                throw;
            }


        }

        public async Task<(Result Result, string UserId)> CreateCustomerUserAsync(CustomerRegsiter register)
        {
            var user = new ApplicationUser
            {
                UserName = register.UserName,
                Email = register.Email,
                PasswordHash = register.Password

            };

            try
            {
                var result = await _userManager.CreateAsync(user, register.Password);

                var role = await _roleManager.FindByNameAsync(UserRoles.CustomerRoleName);
                result = await _userManager.AddToRoleAsync(user, role.NormalizedName);

                return (result.ToApplicationResult(), user.Id);
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                throw;
            }


        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            try
            {
                var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

                return user != null && await _userManager.IsInRoleAsync(user, role);
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                throw;
            }
        }

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            try
            {
                var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    return false;
                }

                var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

                var result = await _authorizationService.AuthorizeAsync(principal, policyName);

                return result.Succeeded;
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                throw;
            }
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            try
            {
                var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
                return user != null ? await DeleteUserAsync(user) : Result.Success();
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                throw;
            }
        }

        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            try
            {
                var result = await _userManager.DeleteAsync(user);
                return result.ToApplicationResult();
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                throw;
            }
        }

        public async Task<UserManagerResponse> Login(Login logIn)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(logIn.UserName);
                var result = await _signInManager.PasswordSignInAsync(logIn.UserName, logIn.Password, false, false);

                var role = await _userManager.GetRolesAsync(user);

                if (result.Succeeded)
                {
                    var claims = new[]
                    {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, role[0]),

                 };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("123456789ThisisarandomKEY123456789"));

                    var token = new JwtSecurityToken(
                    issuer: "http://ahmadmozaffar.net",
                    audience: "http://ahmadmozaffar.net",
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

                    string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

                    return new UserManagerResponse
                    {
                        Token = tokenAsString,
                        IsSuccess = true,
                        ExpireDate = token.ValidTo,
                        UserName = user.NormalizedUserName,
                        UserEmail = user.NormalizedEmail,
                        UserRole = role[0]


                    };

                }

                return null;
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                throw;
            }
        }

        public IList<CustomerDetails> GetUsersInCustomerRole()
        {
            try
            {
                var users = _userManager.GetUsersInRoleAsync("Customer").Result;
                IList<CustomerDetails> customerList = new List<CustomerDetails>();

                //Need to change this implementation with a better approah
                foreach (var item in users)
                {
                    CustomerDetails obj = new CustomerDetails();
                    obj.UserName = item.UserName;
                    obj.Email = item.Email;
                    obj.ContactNumber = item.PhoneNumber;

                    customerList.Add(obj);
                }

                return customerList;
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                throw;
            }
        }
    }
}
