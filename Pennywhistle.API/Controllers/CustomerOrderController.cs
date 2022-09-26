using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pennywhistle.Application;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.Customer.Commands;
using Pennywhistle.Application.Customer.Query;
using System;
using System.Threading.Tasks;

namespace Pennywhistle.API.Controllers
{
    /// <summary>
    /// Implements customer order controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CustomerOrderController : ApiController
    {
        #region Public Variables
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Public Constructor
        public CustomerOrderController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Create order
        /// </summary>
        /// <param name="command">order details</param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand command)
        {
            try
            {
                return Ok(await Mediator.Send(command));
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                return BadRequest(NLogErrorLog.CommonErrorMessage);
            }
        }

        /// <summary>
        /// Get order history for a given customer id
        /// </summary>
        /// <param name="userId">user GUID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOrderHistoryForCustomer")]
        public async Task<IActionResult> GetHistory(string userId)
        {
            try
            {
                //best practice : can return a different view model without exposing the domain entity
                return Ok(await Mediator.Send(new GetAllOrdersForCustomerQuery { UserId = userId }));
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                return BadRequest(NLogErrorLog.CommonErrorMessage);
            }
        }

        /// <summary>
        /// Get customer current order
        /// </summary>
        /// <param name="userId">user GUID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCurrentOrderForCustomer")]
        public async Task<IActionResult> GetCurrentOrder(string userId)
        {
            try
            {
                //best practice : can return a different view model without exposing the domain entity
                return Ok(await Mediator.Send(new GetCustomerCurrentOrderQuery { UserId = userId }));
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
