namespace FriendlyUrlForMvc.Web.Infrastructure.Services.Base {
    /// <summary>
    /// Default settings for service
    /// </summary>
    public class DefaultServiceSettings : IServiceSettings {

        /// <summary>
        /// Enables using Includes for PagedList method if items parameter is null
        /// </summary>
        public bool IncludeWhenPagedListRequested => false;

        /// <summary>
        /// Overrides config settings PageSize for paged list items
        /// </summary>
        public int? PageSizeForPagedList { get { return null; } }
    }
}