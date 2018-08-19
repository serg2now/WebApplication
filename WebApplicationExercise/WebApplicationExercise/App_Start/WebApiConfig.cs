using System;
using System.Web.Http;
using System.Web.Http.Tracing;
using AutoMapper;
using Unity;
using Unity.Lifetime;
using WebApplicationExercise.Core;
using WebApplicationExercise.Converters;
using WebApplicationExercise.Loging;
using WebApplicationExercise.Models.EFModels;
using WebApplicationExercise.Repositories;
using WebApplicationExercise.Models.ContractModels;
using WebApplicationExercise.Models.ModelMapper;

namespace WebApplicationExercise
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{version}/{controller}/{id}",
                defaults: new { version = "v1", id = RouteParameter.Optional }
            );

            var container = new UnityContainer();
            
            RegisterTypes(container);
            RegisterMapper(container);

            config.DependencyResolver = new UnityResolver(container);

            GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), container.Resolve<ITraceWriter>());

            config.Filters.Add(container.Resolve<LoggingFilterAttribute>());
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IRepository<DbOrder>, OrdersRepository>(new HierarchicalLifetimeManager());
            container.RegisterType(typeof(CustomerManager), new HierarchicalLifetimeManager());
            container.RegisterType<IMapper<DbOrder, Order>, OrderMapper>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMapper<DbProduct, Product>, ProductMapper>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITraceWriter, NLogLogger>(new ContainerControlledLifetimeManager());
            container.RegisterType(typeof(MainDataContext), new HierarchicalLifetimeManager());
            container.RegisterType(typeof(LoggingFilterAttribute), new HierarchicalLifetimeManager());
            container.RegisterType<ICurrencyConverter, CurrencyConverter>(new HierarchicalLifetimeManager());
        }

        private static void RegisterMapper(IUnityContainer container)
        {
            var config = new MapperConfiguration(cfg =>
            {
                var orderConfigurator = container.Resolve<IMapper<DbOrder, Order>>();
                orderConfigurator.ConfigureMapper(cfg);

                var productConfigurator = container.Resolve<IMapper<DbProduct, Product>>();
                productConfigurator.ConfigureMapper(cfg);
            });

            IMapper mapper = config.CreateMapper();

            container.RegisterInstance(mapper);
        }
    }
}
