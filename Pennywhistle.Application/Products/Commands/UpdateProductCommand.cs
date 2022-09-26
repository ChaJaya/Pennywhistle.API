using MediatR;

namespace Pennywhistle.Application.Products.Commands
{
    /// <summary>
    /// Update product command
    /// </summary>
    public class UpdateProductCommand : IRequest<int>
    {
        public UpdateProductCommand()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        //public string FullSku { get; set; }
        public string Size { get; set; } // will get validated from client end
        public double Price { get; set; }
    }
}
