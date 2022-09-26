using AutoMapper;
using Pennywhistle.Application.Products.Commands;
using Pennywhistle.Domain.Entities;

namespace Pennywhistle.Application.Products.MappingProfile
{
    /// <summary>
    /// Implements mapping for product
    /// </summary>
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<CreateProductCommand, Product>();
            CreateMap<UpdateProductCommand, Product>();
        }
    }
}
