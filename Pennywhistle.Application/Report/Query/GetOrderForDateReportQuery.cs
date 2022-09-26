using MediatR;
using Pennywhistle.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Pennywhistle.Application.Report.Query
{
    /// <summary>
    /// Get order for date request
    /// </summary>
    public class GetOrderForDateReportQuery : IRequest<IList<Order>>
    {
        public DateTime ForThisDate { get; set; }
        public int OrderStatus { get; set; }
    }
}
