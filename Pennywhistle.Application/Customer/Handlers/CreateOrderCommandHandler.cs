using AutoMapper;
using MediatR;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.Common.Enum;
using Pennywhistle.Application.Customer.Commands;
using Pennywhistle.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pennywhistle.Application.Customer.Handlers
{
    /// <summary>
    /// Implements create order command
    /// </summary>
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        #region Private variables
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        #endregion

        #region Public Constructor
        public CreateOrderCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        } 
        #endregion

        /// <summary>
        /// implements order handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //can add this to a separate repository
                var entity = _mapper.Map<Order>(request);
                entity.ProductItemId = request.ProductItemId;
                entity.OrderStatus = (int)OrderStatus.PendingOrder;

                _context.Orders.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return entity.Id;
            }
            catch (Exception ex)
            {
                NLogErrorLog.LogErrorMessages(ex.Message);
                throw;
            }
        }
    }
}
