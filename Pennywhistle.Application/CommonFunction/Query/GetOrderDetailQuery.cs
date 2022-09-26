using MediatR;
using Pennywhistle.Domain.Entities;

namespace Pennywhistle.Application.CommonFunction.Query
{
    /// <summary>
    /// Order details query request
    /// </summary>
    public class GetOrderDetailQuery : IRequest<Order>
    {
        public int OrderId { get; set; }
    }
}
