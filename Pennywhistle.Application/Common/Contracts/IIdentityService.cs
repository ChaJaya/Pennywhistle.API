using Pennywhistle.Application.Common.Models;
using Pennywhistle.Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pennywhistle.Application.Common.Contracts
{
    /// <summary>
    /// Identity contact
    /// </summary>
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<(Result Result, string UserId)> CreateUserAsync(Register register);
        Task<(Result Result, string UserId)> CreateCustomerUserAsync(CustomerRegsiter register);

        Task<Result> DeleteUserAsync(string userId);

        Task<UserManagerResponse> Login(Login logIn);
        IList<CustomerDetails> GetUsersInCustomerRole();
    }
}
