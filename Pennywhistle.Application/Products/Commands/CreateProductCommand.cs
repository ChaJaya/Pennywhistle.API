using MediatR;

namespace Pennywhistle.Application.Products.Commands
{
    /// <summary>
    /// Implements creta product command
    /// </summary>
    public class CreateProductCommand : IRequest<int>
    {
        public CreateProductCommand()
        {

        }

        public string Name { get; set; }
        public string Sku { get; set; }
        public string Size { get; set; } // will get validated from client end
        public double Price { get; set; }
    }
}
