using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.Common.Enum;
using Pennywhistle.Application.Customer.Query;
using Pennywhistle.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pennywhistle.Application.Customer.Handlers
{
    /// <summary>
    /// Implements ccutomer currenr order handler
    /// </summary>
    public class GetCustomerCurrentOrderHandler : IRequestHandler<GetCustomerCurrentOrderQuery, Order>
    {
        #region Private variables
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        #endregion

        #region Public constructor
        public GetCustomerCurrentOrderHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Implements handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Order> Handle(GetCustomerCurrentOrderQuery request, CancellationToken cancellationToken)
        {
            var result = new Order();
            try
            {
                //can add this to a separate repository
                var data = await _context.Orders.Where(a => a.UserId.Value == new Guid(request.UserId) && (a.OrderStatus != (int)OrderStatus.OrderComplete && a.OrderStatus != (int)OrderStatus.CancelledOrder)).Include(s => s.Products).FirstOrDefaultAsync();
                if (data != null)
                {
                    result = _mapper.Map<Order>(data);
                }
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                throw;
            }

            return result;
        }
        #endregion
    }
}
