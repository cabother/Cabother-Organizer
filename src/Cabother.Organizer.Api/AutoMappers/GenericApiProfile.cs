using AutoMapper;
using Cabother.Organizer.Api.ViewModels;
using Cabother.Organizer.Application.ViewModels;

namespace Cabother.Organizer.Api.AutoMappers
{
    public class GenericApiProfile : Profile
    {
        public GenericApiProfile()
        {
            CreateMap(typeof(PagedListViewModel<>), typeof(MetadataViewModel))
                .ForMember("Server", opt => opt.Ignore());

            CreateMap(typeof(PagedListViewModel<>), typeof(SuccessResponseViewModel<>))
                .ForMember("Meta", opt => opt.MapFrom(src => src));
        }
    }
}