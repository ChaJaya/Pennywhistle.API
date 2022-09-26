using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pennywhistle.Application;
using Pennywhistle.Application.CommonFunction.Command;
using Pennywhistle.Application.CommonFunction.Query;
using System;
using System.Threading.Tasks;

namespace Pennywhistle.API.Controllers
{
    /// <summary>
    /// Implements commmon controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommonController : ApiController
    {
        #region Public Methods

        /// <summary>
        /// Get order detail for given order id
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOrderDetail")]
        public async Task<IActionResult> GetOrderDetail(int orderId)
        {
            try
            {
                //best practice : can return a different view model without exposing the domain entity
                return Ok(await Mediator.Send(new GetOrderDetailQuery { OrderId = orderId }));
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                return BadRequest(NLogErrorLog.CommonErrorMessage);
            }
        }

        /// <summary>
        /// Update order details by staff
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateOrderStatus")]
        [Authorize(Roles = "Admin,DeliveryStaff,KitchenStaff,StoreStaff")]
        public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatusCommand command)
        {
            try
            {
                //best practice : can return a different view model without exposing the domain entity
                return Ok(await Mediator.Send(command));
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
