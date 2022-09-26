using MediatR;
using Pennywhistle.Domain.Entities;
using System.Collections.Generic;

namespace Pennywhistle.Application.Customer.Query
{
    /// <summary>
    /// Implemenys customer all order request
    /// </summary>
    public class GetAllOrdersForCustomerQuery : IRequest<IList<Order>>
    {
        public string UserId { get; set; }
    }
}
