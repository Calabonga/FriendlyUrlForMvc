using AutoMapper;
using FriendlyUrlForMvc.Data;
using FriendlyUrlForMvc.Models;
using FriendlyUrlForMvc.Web.Infrastructure.QueryParams;
using FriendlyUrlForMvc.Web.Infrastructure.Services;
using FriendlyUrlForMvc.Web.Infrastructure.Services.Base;
using FriendlyUrlForMvc.Web.Models;

namespace FriendlyUrlForMvc.Web.Infrastructure.Repository {
    public class EditablePageRepository : WritableRepositoryBase<EditablePage, EditablePageUpdateViewModel, EditablePage, PagedListQueryParams> {
        public EditablePageRepository(IContext context, IAppConfig config, IMapper mapper, IServiceSettings settings, ILogService logger)
            : base(context, config, mapper, settings, logger) {
        }

    }
}
