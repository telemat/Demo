namespace FlickrApp
{
    #region Imports

    using Autofac;
    using Core;

    #endregion

    public class WindowsStartup
    {
        public void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new CoreModule());

            var container = builder.Build();

            Resolver.Instance.RegisterContainer(container);
        }
    }
}