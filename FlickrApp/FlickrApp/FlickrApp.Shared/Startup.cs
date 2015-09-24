namespace FlickrApp
{
    #region Imports

    using Autofac;
    using Core;
    using ViewModels;

    #endregion

    internal sealed class Startup
    {
        public void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new CoreModule());

            // viewmodels
            builder.RegisterType<SearchPageViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<PivotPageViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<LocationPageViewModel>().AsSelf().SingleInstance();

            var container = builder.Build();

            Resolver.Instance.RegisterContainer(container);
        }
    }
}