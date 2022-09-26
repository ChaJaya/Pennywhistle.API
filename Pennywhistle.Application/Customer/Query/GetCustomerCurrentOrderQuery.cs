using MediatR;
using Pennywhistle.Domain.Entities;

namespace Pennywhistle.Application.Customer.Query
{
    /// <summary>
    /// Implemeys customer current order request
    /// </summary>
    public class GetCustomerCurrentOrderQuery : IRequest<Order>
    {
        public string UserId { get; set; }
    }
}
