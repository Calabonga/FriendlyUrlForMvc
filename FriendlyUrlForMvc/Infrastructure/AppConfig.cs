using Calabonga.Portal.Config;

namespace FriendlyUrlForMvc.Web.Infrastructure {
    public interface IAppConfig {
        CurrentAppSettings Config { get; }
    }

    public class AppConfig : ConfigServiceBase<CurrentAppSettings>, IAppConfig {
        public AppConfig(IConfigSerializer serializer, ICacheService cacheService)
            : base(serializer, cacheService) {
        }

        public AppConfig(string configFileName, IConfigSerializer serializer, ICacheService cacheService)
            : base(configFileName, serializer, cacheService) {
        }
    }
}
