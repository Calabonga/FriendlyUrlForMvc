using AutoMapper;
using Calabonga.PagedListLite;

namespace FriendlyUrlForMvc.Web.Infrastructure.Mapper.Base {

    /// <summary>
    /// Base class for mapper configuration.
    /// All ViewModel that will be mapped should implement IAutoMapper
    /// </summary>
    public abstract class MapperConfigurationBase : Profile, IAutoMapper { }


    public abstract class MapperConfigurationBase<TModel, TViewModel> : Profile, IAutoMapper {
        protected MapperConfigurationBase() {
            CreateMap<TModel, TViewModel>();

            CreateMap<PagedList<TModel>, PagedList<TViewModel>>()
               .ConvertUsing<PagedListConverter<TModel, TViewModel>>();
        }
    }
}
