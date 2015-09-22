namespace FlickrApp
{
    #region Imports

    using Autofac;

    #endregion

    /// <summary>
    /// Resolves interfaces registered by the DI container
    /// </summary>
    internal sealed class Resolver
    {
        #region Singleton

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static Resolver()
        {
        }

        private Resolver()
        {
        }

        public static Resolver Instance { get; } = new Resolver();

        #endregion

        private IContainer _container;

        public void RegisterContainer(IContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}