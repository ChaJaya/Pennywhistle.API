using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pennywhistle.Application;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.Products.Commands;
using Pennywhistle.Application.Products.Query;
using System;
using System.Threading.Tasks;

namespace Pennywhistle.API.Controllers
{
    /// <summary>
    /// Implements product controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductController : ApiController
    {
        #region Private variables
        private readonly ICurrentUserService _currentUserService;

        #endregion

        #region Public Cosntructor
        public ProductController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Create product item
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateProductItem")]
        public async Task<IActionResult> CreateProductItem(CreateProductCommand command)
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
        /// Update product item
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateProductItem")]
        public async Task<IActionResult> UpdateProductItem(UpdateProductCommand command)
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
        /// Get all products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {

                //best practice : can return a different view model without exposing the domain entity
                return Ok(await Mediator.Send(new GetAllProductQuery()));
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                return BadRequest(NLogErrorLog.CommonErrorMessage);
            }
        }

        /// <summary>
        /// Delete product item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteProductItem")]
        public async Task<IActionResult> DeleteProductItem([FromBody] int id)
        {
            try
            {
                return Ok(await Mediator.Send(new DeleteProductCommand { Id = id }));
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
