using System;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;

namespace FriendlyUrlForMvc.Web.Infrastructure.Mapper.Base {
    /// <summary>
    /// AutoMapper profiles registration
    /// </summary>
    public static class MapperRegistration {

        /// <summary>
        /// Creates a Mapper instance
        /// </summary>
        public static void Register(ContainerBuilder builder) {
            // gathering all mapper's profiles
            var profiles =
                (from t in typeof(MvcApplication).GetTypeInfo().Assembly.GetTypes()
                 where typeof(IAutoMapper).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract
                 select t).ToList();


            if (profiles.Any()) {
                var config = new MapperConfiguration(cfg => {
                    foreach (var item in profiles.Select(profile => (Profile)Activator.CreateInstance(profile))) {
                        cfg.AddProfile(item);
                    }
                });
                config.AssertConfigurationIsValid();
                builder.Register(ctx => config);

                builder.Register(c => {
                    var context = c.Resolve<IComponentContext>();
                    var configMapper = context.Resolve<MapperConfiguration>();
                    return configMapper.CreateMapper(x => context.Resolve(x));

                }).As<IMapper>();
            }
            else {
                throw new InvalidOperationException("Profiles for mapping not found");
            }
        }
    }
}