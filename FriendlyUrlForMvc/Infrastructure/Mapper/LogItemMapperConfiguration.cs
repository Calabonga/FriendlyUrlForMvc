using Calabonga.PagedListLite;
using FriendlyUrlForMvc.Models;
using FriendlyUrlForMvc.Web.Infrastructure.Dto;
using FriendlyUrlForMvc.Web.Infrastructure.Mapper.Base;

namespace FriendlyUrlForMvc.Web.Infrastructure.Mapper {
    /// <summary>
    /// Mapper configuration for AccidentType entity
    /// </summary>
    public class LogItemMapperConfiguration : MapperConfigurationBase {

        public LogItemMapperConfiguration() {
            CreateMap<LogItem, LogItemDto>()
                .ForMember(x => x.LogType, o => o.MapFrom(c => c.LogType.ToString()));


            CreateMap<PagedList<LogItem>, PagedList<LogItemDto>>()
                .ConvertUsing<PagedListConverter<LogItem, LogItemDto>>();

        }
    }
}