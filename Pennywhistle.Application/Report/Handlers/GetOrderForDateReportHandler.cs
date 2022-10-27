using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.Report.Query;
using Pennywhistle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pennywhistle.Application.Report.Handlers
{
    /// <summary>
    /// Get orders for date handler
    /// </summary>
    public class GetOrderForDateReportHandler : IRequestHandler<GetOrderForDateReportQuery, IList<Order>>
    {
        #region Private variables

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        #endregion

        #region Public constructor
        public GetOrderForDateReportHandler(IApplicationDbContext context, IMapper mapper)
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
        public async Task<IList<Order>> Handle(GetOrderForDateReportQuery request, CancellationToken cancellationToken)
        {

            //can add this to a separate repository
            if (request.OrderStatus == -1)
            {
                var result = new List<Order>();
                var data = await _context.Orders.Where(a => a.Created.Date == request.ForThisDate.Date).ToListAsync();
                if (data != null)
                {
                    result = _mapper.Map<List<Order>>(data);
                }

                return result;
            }
            else
            {
                var result = new List<Order>();
                var data = await _context.Orders.Where(a => a.Created.Date == request.ForThisDate.Date && request.OrderStatus == a.OrderStatus).ToListAsync();
                if (data != null)
                {
                    result = _mapper.Map<List<Order>>(data);
                }

                return result;
            }

        }
        #endregion
    }
}
