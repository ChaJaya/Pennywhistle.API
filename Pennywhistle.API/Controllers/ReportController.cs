using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pennywhistle.Application;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.Common.Variables;
using Pennywhistle.Application.Customer.Query;
using Pennywhistle.Application.Helpers;
using Pennywhistle.Application.Report.Query;
using System;
using System.Threading.Tasks;

namespace Pennywhistle.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ReportController : ApiController
    {
        #region Public varibales/ properties
        public IIdentityService _identityService { get; }
        #endregion

        #region Public Constructor
        public ReportController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Return order list for given date and order status (Pending , cancel , all)
        /// </summary>
        /// <param name="date">set only date part (2022-09-23)</param>
        /// <param name="orderStatus"> pass -1 for orderStatus for all orders</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOrderReport")]
        public async Task<IActionResult> GetOrderReport(string date, int orderStatus)
        {
            try
            {
                var data = await Mediator.Send(new GetOrderForDateReportQuery { ForThisDate = Convert.ToDateTime(date).Date, OrderStatus = orderStatus });
                var excelData = ExcelGenerator.GenerateExcelFromList(data, "report").ReadAsByteArrayAsync().Result;
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Order.xlsx");
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                return BadRequest(NLogErrorLog.CommonErrorMessage);
            }
        }

        /// <summary>
        /// Get customer order history report
        /// </summary>
        /// <param name="userId">pass GUID of user as string</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOrderHistoryForCustomerReport")]
        public async Task<IActionResult> GetOrderHistoryForCustomerReport(string userId)
        {

            var data = await Mediator.Send(new GetAllOrdersForCustomerQuery { UserId = userId });
            var excelData = ExcelGenerator.GenerateExcelFromList(data, "report").ReadAsByteArrayAsync().Result;
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomerOrderHistory.xlsx");

        }

        /// <summary>
        /// Get customer details report
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCustomerDetailsReport")]
        public async Task<IActionResult> GetCustomerDetailsReport()
        {
            var data = _identityService.GetUsersInCustomerRole();
            var excelData = ExcelGenerator.GenerateExcelFromList(data, "report").ReadAsByteArrayAsync().Result;
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RegisterdCustomers.xlsx");
        }
        #endregion
    }
}
