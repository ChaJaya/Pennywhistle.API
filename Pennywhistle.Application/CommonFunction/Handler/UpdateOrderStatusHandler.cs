using AutoMapper;
using MediatR;
using Pennywhistle.Application.Common.Contracts;
using Pennywhistle.Application.Common.Enum;
using Pennywhistle.Application.Common.Variables;
using Pennywhistle.Application.CommonFunction.Command;
using Pennywhistle.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pennywhistle.Application.CommonFunction.Handler
{
    /// <summary>
    /// Implements UpdateOrderStatusHandler
    /// </summary>
    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, Order>
    {
        #region Private Variables
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _mailService;
        #endregion

        #region Public Constructor
        public UpdateOrderStatusHandler(IApplicationDbContext context, IMapper mapper, IEmailService mailService)
        {
            _context = context;
            _mapper = mapper;
            _mailService = mailService;
        }
        #endregion

        #region Public Methods
        public async Task<Order> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {

            //can add this to a separate repository
            var order = _context.Orders.Where(a => a.Id == request.OrderId).FirstOrDefault();

            if (order == null)
            {
                return default;
            }
            else
            {
                //With the time if business grows to a higer level ,and if we have to add more roles and order status details
                // we will have to refator this code since this will violate open close principle
                if (request.LoggedInUserRole == UserRoles.AdminRoleName)
                {
                    order.OrderStatus = request.OrderStatus;
                }
                if (request.LoggedInUserRole == UserRoles.StoreStaffRoleName)
                {
                    order.OrderStatus = (int)OrderStatus.CancelledOrder;
                }
                if (request.LoggedInUserRole == UserRoles.KitchenStaffRoleName)
                {
                    if (request.OrderStatus == (int)OrderStatus.PrepareOrder)
                    {
                        order.OrderStatus = (int)OrderStatus.PrepareOrder;
                    }
                    if (request.OrderStatus == (int)OrderStatus.ReadyToPickUp)
                    {
                        order.OrderStatus = (int)OrderStatus.ReadyToPickUp;
                    }

                }
                if (request.LoggedInUserRole == UserRoles.DeliveryStaffRoleName)
                {
                    order.OrderStatus = (int)OrderStatus.OrderDelivered;
                }

                if ((request.LoggedInUserRole == UserRoles.AdminRoleName) && (order.OrderStatus == 5))
                {
                    order.OrderStatus = (int)OrderStatus.OrderComplete;
                }


                await _context.SaveChangesAsync(cancellationToken);

                _mailService.SendEmail(request.CustomerEmail);

                return order;
            }



        }
        #endregion
    }
}
