using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pennywhistle.Application;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.Common.Models;
using System;
using System.Threading.Tasks;

namespace Pennywhistle.API.Controllers
{
    /// <summary>
    /// Implements login controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ApiController
    {
        #region Variables
        public IIdentityService _identityService { get; }
        #endregion

        #region Public Constructor
        public LoginController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This will login user to system
        /// </summary>
        /// <param name="logIn">login details</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(Login logIn)
        {
            try
            {
                var resultData = await _identityService.Login(logIn);
                return Ok(resultData);
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                return BadRequest(NLogErrorLog.CommonErrorMessage);
            }
        }

        /// <summary>
        /// This end point will create Customer
        /// </summary>
        /// <param name="register">register object with details</param>
        /// <returns>returns user GUID</returns>
        [HttpPost]
        [Route("CreateCustomer")]
        public async Task<IActionResult> CreateCustomerUserAsync(CustomerRegsiter register)
        {
            try
            {
                var resultData = await _identityService.CreateCustomerUserAsync(register);
                return Ok(resultData.UserId);
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                return BadRequest(NLogErrorLog.CommonErrorMessage);
            }
        } 
        #endregion
    }
}
