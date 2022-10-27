using AutoMapper;
using MediatR;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.Products.Commands;
using Pennywhistle.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pennywhistle.Application.Products.Handlers
{
    /// <summary>
    /// Create product handler
    /// </summary>
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        #region Private variables
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        #endregion

        #region Public constructor
        public CreateProductCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Create product handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {

            //can add this to a separate repository
            var entity = _mapper.Map<Product>(request);

            _context.Products.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;

        }
        #endregion
    }
}
