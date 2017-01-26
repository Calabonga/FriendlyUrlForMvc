using Calabonga.Portal.Config;

namespace FriendlyUrlForMvc.Web.Infrastructure {
    public class CurrentAppSettings : AppSettings {

        /// <summary>
        /// Название компании в отчетных документах и в уведомительных письмах
        /// </summary>
        public string CompanyName { get; set; }

    }
}