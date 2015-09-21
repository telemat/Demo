namespace FlickrApp.Core
{
    #region Imports

    using Autofac;
    using Contracts;
    using Services;

    #endregion

    public class CoreModule
        : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //
            // Singletons
            //
            builder.RegisterType<FlickrService>().As<IFlickrService>().SingleInstance();

            base.Load(builder);
        }
    }
}