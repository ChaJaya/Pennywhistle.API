using AutoMapper;
using Pennywhistle.Application.Customer.Commands;
using Pennywhistle.Domain.Entities;

namespace Pennywhistle.Application.Customer.MappingProfile
{
    /// <summary>
    /// Impelmenys Customer order mapping
    /// </summary>
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<CreateOrderCommand, Order>();
        }
    
    }
}
