using FriendlyUrlForMvc.Models;
using FriendlyUrlForMvc.Web.Infrastructure.Mapper.Base;
using FriendlyUrlForMvc.Web.Models;

namespace FriendlyUrlForMvc.Web.Infrastructure.Mapper {
    /// <summary>
    /// Mapper configuration for EditablePage entity
    /// </summary>
    public class EditablePageMapperConfiguration : MapperConfigurationBase {

        public EditablePageMapperConfiguration() {
            CreateMap<EditablePage, EditablePageUpdateViewModel>();
            CreateMap<EditablePageUpdateViewModel, EditablePage>()
                .ForMember(x => x.CreatedAt, o => o.Ignore())
                .ForMember(x => x.CreatedBy, o => o.Ignore())
                .ForMember(x => x.UpdatedAt, o => o.Ignore())
                .ForMember(x => x.UpdatedBy, o => o.Ignore());

        }
    }
}