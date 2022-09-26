using MediatR;
using Pennywhistle.Domain.Entities;
using System.Collections.Generic;

namespace Pennywhistle.Application.Products.Query
{
    /// <summary>
    /// Get all products request
    /// </summary>
    public class GetAllProductQuery : IRequest<IList<Product>>
    {
      
    }
}
