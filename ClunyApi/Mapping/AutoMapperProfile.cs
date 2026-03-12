using AutoMapper;
using Shared.Models;

namespace ClunyApi.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //CreateMap<Product, ProductDto>();
            //CreateMap<ProductCreateDto, Product>()
            //    .ForMember(dest => dest.Id, opt => opt.Ignore())
            //    .ForMember(dest => dest.Category, opt => opt.Ignore());

            //CreateMap<Category, CategorySummaryDto>().ReverseMap();
        }
    }
}
