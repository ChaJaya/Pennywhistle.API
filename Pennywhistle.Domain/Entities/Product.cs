using Pennywhistle.Domain.Common;


namespace Pennywhistle.Domain.Entities
{
    /// <summary>
    /// Product domain entity
    /// </summary>
    public class Product : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }

        public string FullSku
        {
            get
            {
                return Sku + Size;
            }

            set
            {

            }
        }
        public string Size { get; set; } // will get validated from client end
        public double Price { get; set; }
    }
}
