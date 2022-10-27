using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.Products.Commands;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pennywhistle.Application.Products.Handlers
{
    /// <summary>
    /// implements update product handler
    /// </summary>
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
    {
        #region Private variables
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        #endregion

        #region Public Constructor
        public UpdateProductCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Implements handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {

            //can add this to a separate repository
            var product = await _context.Products.Where(a => a.Id == request.Id).FirstOrDefaultAsync();

            if (product == null)
            {
                return default;
            }
            else
            {
                product.Name = request.Name;
                // product.FullSku = request.FullSku;
                product.Sku = request.Sku;
                product.Size = request.Size;
                product.Price = request.Price;

                await _context.SaveChangesAsync(cancellationToken);
                return product.Id;
            }

        }
        #endregion
    }
}
