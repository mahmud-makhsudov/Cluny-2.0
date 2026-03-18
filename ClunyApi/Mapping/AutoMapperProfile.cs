using AutoMapper;
using Shared.Dtos;
using Shared.Models;

namespace ClunyApi.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UpdateCategoryDto, Category>();

            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

            CreateMap<CreateOrderDto, Order>();
            CreateMap<UpdateOrderDto, Order>();

            CreateMap<CreateOptionGroupDto, OptionGroup>();
            CreateMap<UpdateOptionGroupDto, OptionGroup>();

            CreateMap<CreateOptionDto, Option>();
            CreateMap<UpdateOptionDto, Option>();
        }
    }
}
