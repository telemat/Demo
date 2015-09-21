namespace FlickrApp
{
    #region Imports

    using Autofac;

    #endregion

    /// <summary>
    /// Locator used for resolving interfaces
    /// </summary>
    internal sealed class Locator
    {
        #region Singleton

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static Locator()
        {
        }

        private Locator()
        {
        }

        public static Locator Instance { get; } = new Locator();

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