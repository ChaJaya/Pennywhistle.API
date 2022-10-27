using AutoMapper;
using MediatR;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.Products.Commands;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pennywhistle.Application.Products.Handlers
{
    /// <summary>
    /// Delete product by id handler
    /// </summary>
    public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductCommand, int>
    {
        #region Private variables
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        #endregion

        #region Public Constructor
        public DeleteProductByIdCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Implement handler
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {

            //can add this to a separate repository
            var product = _context.Products.Where(a => a.Id == command.Id).FirstOrDefault();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
            return product.Id;
        }
        #endregion
    }
}
