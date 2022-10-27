using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.Customer.Query;
using Pennywhistle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pennywhistle.Application.Customer.Handlers
{
    /// <summary>
    /// Implements get all arders for customer handler
    /// </summary>
    public class GetAllOrdersForCustomerHandler : IRequestHandler<GetAllOrdersForCustomerQuery, IList<Order>>
    {
        #region Private variables

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        #endregion

        #region Public Constuctor
        public GetAllOrdersForCustomerHandler(IApplicationDbContext context, IMapper mapper)
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
        public async Task<IList<Order>> Handle(GetAllOrdersForCustomerQuery request, CancellationToken cancellationToken)
        {
            var result = new List<Order>();

            //can add this to a separate repository
            var data = await _context.Orders.Where(a => a.UserId.Value == new Guid(request.UserId)).ToListAsync();
            if (data != null)
            {
                result = _mapper.Map<List<Order>>(data);
            }

            return result;
        }
        #endregion
    }
}
