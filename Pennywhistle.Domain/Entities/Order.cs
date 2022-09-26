using Pennywhistle.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pennywhistle.Domain.Entities
{
    /// <summary>
    /// Order domain entity
    /// </summary>
    public class Order : BaseEntity
    {
        //public Order()
        //{
        //    this.ProductItem = new Product(); // Assume 1 order can contain only 1 product
        //}
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string FullSku { get; set; }
        public string Size { get; set; }
        public int OrderStatus { get; set; }
        public double TotalCost { get; set; }

        [ForeignKey("Products")]
        public int ProductItemId { get; set; }
        public Product Products { get; set; }
      
    }
}
