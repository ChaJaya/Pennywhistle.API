using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.Products.Query;
using Pennywhistle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pennywhistle.Application.Products.Handlers
{
    /// <summary>
    /// Get all product handler
    /// </summary>
    public class GetAllProductsHandler : IRequestHandler<GetAllProductQuery, IList<Product>>
    {
        #region Private variables
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        #endregion

        #region Public Constructor
        public GetAllProductsHandler(IApplicationDbContext context, IMapper mapper)
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
        public async Task<IList<Product>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var result = new List<Product>();

            //can add this to a separate repository
            var data = await _context.Products.ToListAsync();
            if (data != null)
            {
                result = _mapper.Map<List<Product>>(data);
            }

            return result;
        }
        #endregion
    }
}

