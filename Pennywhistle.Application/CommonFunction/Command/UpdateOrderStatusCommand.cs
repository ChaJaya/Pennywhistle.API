using MediatR;
using Pennywhistle.Domain.Entities;

namespace Pennywhistle.Application.CommonFunction.Command
{
    /// <summary>
    /// Order status command
    /// </summary>
    public class UpdateOrderStatusCommand : IRequest<Order>
    {
        public int OrderId { get; set; }
        public int OrderStatus { get; set; }
        public string LoggedInUserRole { get; set; }
        public string CustomerEmail { get; set; }
    }
}
