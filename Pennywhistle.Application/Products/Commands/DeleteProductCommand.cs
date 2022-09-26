using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pennywhistle.Application.Products.Commands
{
    /// <summary>
    /// Delete product command
    /// </summary>
    public class DeleteProductCommand : IRequest<int>
    {
        public int Id { get; set; }
       
    }
}
