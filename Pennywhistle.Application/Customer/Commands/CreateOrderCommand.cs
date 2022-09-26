using MediatR;
using Pennywhistle.Domain.Entities;
using System;

namespace Pennywhistle.Application.Customer.Commands
{
    public class CreateOrderCommand : IRequest<int>
    {
        public CreateOrderCommand()
        {

        }

    
        public Guid? UserId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string FullSku { get; set; }
        public string Size { get; set; }
        public double TotalCost { get; set; }
        public int ProductItemId { get; set; }
    }
}
