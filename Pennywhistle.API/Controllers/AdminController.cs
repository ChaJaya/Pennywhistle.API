using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pennywhistle.Application;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.Common.Models;
using System;
using System.Threading.Tasks;

namespace Pennywhistle.API.Controllers
{
    /// <summary>
    /// Implements admin controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ApiController
    {
        #region Public properties/Variables
        public IIdentityService _identityService { get; }
        #endregion

        #region Public Constructor
        public AdminController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// This end point will create employee users
        /// </summary>
        /// <param name="register">register object with details</param>
        /// <returns>returns user GUID</returns>
        [HttpPost]
        [Route("CreateEmployeeUser")]
        public async Task<IActionResult> CreateEmployeeUser(Register register)
        {
            try
            {
                var resultData = await _identityService.CreateUserAsync(register);
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
