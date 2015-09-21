namespace FlickrApp
{
    #region Imports

    using Autofac;
    using Core;

    #endregion

    public class WindowsPhoneStartup
    {
        public void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new CoreModule());

            Globals.Container = builder.Build();
        }
    }
}