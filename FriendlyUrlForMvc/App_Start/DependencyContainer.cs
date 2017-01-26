using System.Data.Entity;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Calabonga.Portal.Config;
using FriendlyUrlForMvc.Data;
using FriendlyUrlForMvc.Web.Infrastructure;
using FriendlyUrlForMvc.Web.Infrastructure.Mapper.Base;
using FriendlyUrlForMvc.Web.Infrastructure.Repository;
using log4net;

namespace FriendlyUrlForMvc.Web {
    public static class DependencyContainer {

        public static void Initialize() {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly).AsImplementedInterfaces();
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterFilterProvider();

            builder.RegisterType<ApplicationDbContext>().As<IContext>().InstancePerRequest();

            builder.RegisterType<EditablePageRepository>().AsSelf();
            builder.RegisterType<AppConfig>().As<IAppConfig>().InstancePerRequest();
            builder.RegisterType<CacheService>().As<ICacheService>().InstancePerRequest();

            builder.RegisterType<JsonConfigSerializer>().As<IConfigSerializer>().InstancePerRequest();
            builder.RegisterType<CacheService>().As<ICacheService>().InstancePerRequest();


            MapperRegistration.Register(builder);

            var logger = LogManager.GetLogger(typeof(MvcApplication));
            builder.Register(c => logger).As<ILog>().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Name.EndsWith("Processor"))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Name.EndsWith("Repository"))
                .AsSelf()
                .InstancePerRequest();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
