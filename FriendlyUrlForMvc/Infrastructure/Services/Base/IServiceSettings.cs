namespace FriendlyUrlForMvc.Web.Infrastructure.Services.Base {
    /// <summary>
    /// Default settings for service interface
    /// </summary>
    public interface IServiceSettings {

        /// <summary>
        /// Enables using Includes for PagedList method if items parameter is null
        /// </summary>
        bool IncludeWhenPagedListRequested { get; }

        /// <summary>
        /// Overrides config settings PageSize for paged list items
        /// </summary>
        /// <remarks>Return NULL to use default value from AppConfig</remarks>
        int? PageSizeForPagedList { get; }
    }
}