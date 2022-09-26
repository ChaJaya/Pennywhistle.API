using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.CommonFunction.Query;
using Pennywhistle.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pennywhistle.Application.CommonFunction.Handler
{
    /// <summary>
    /// Implements GetOrderDetailHandler
    /// </summary>
    public class GetOrderDetailHandler : IRequestHandler<GetOrderDetailQuery, Order>
    {
        #region Private Variables
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        #endregion

        #region Public Constructor
        public GetOrderDetailHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods
        public async Task<Order> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
        {
            var result = new Order();
            try
            {
                //can add this to a separate repository
                var data = await _context.Orders.Where(a => a.Id == request.OrderId).Include(s => s.Products).SingleOrDefaultAsync();
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
