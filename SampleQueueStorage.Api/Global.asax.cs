using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using SampleQueueStorage.Core.ExceptionHandlers;
using SampleQueueStorage.Core.Infrastructure.Repositories;
using SampleQueueStorage.Core.Infrastructure.Repositories.Contracts;
using SampleQueueStorage.Core.Infrastructure.SessionStorage;
using SampleQueueStorage.Core.Services;

namespace SampleQueueStorage.Api
{
    using SampleQueueStorage.Core.Infrastructure.UoW;
    using System.Web.Http;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //UnityConfig.RegisterComponents(GlobalConfiguration.Configuration);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();

            //builder.RegisterGeneric(typeof(IRepository<>), typeof(Repository<,>));         

            builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>)).InstancePerDependency();
            builder.RegisterType<MessageService>().As<IMessageService>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            //container.RegisterInstance<IDbContextStorage>(new WebSessionStorage());
            builder.RegisterType<WebSessionStorage>().As<IDbContextStorage>().InstancePerDependency();
            builder.RegisterType<ExceptionHandler>().As<IExceptionHandler>();
            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}