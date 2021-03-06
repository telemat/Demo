﻿ // The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace FlickrApp
{
    #region Imports

    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;
    using Common;
    using ViewModels;

    #endregion

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PivotPage : Page
    {
        public class Parameter
        {
            public ReadOnlyObservableCollection<PhotoViewModel> Photos { get; }

            public PhotoViewModel Selection { get; }

            public Parameter(ObservableCollection<PhotoViewModel> photos, PhotoViewModel selection)
            {
                Photos = new ReadOnlyObservableCollection<PhotoViewModel>(photos);
                Selection = selection;
            }
        }

        public PivotPageViewModel ViewModel { get; }

        public PivotPage()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;

            if (! Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                if (ViewModel == null)
                {
                    ViewModel = Resolver.Instance.Resolve<PivotPageViewModel>();
                    DataContext = ViewModel;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper { get; }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                var param = e.Parameter as Parameter;

                Debug.Assert(param != null);
                Debug.Assert(ViewModel != null);

                ViewModel.Photos = param.Photos;
                ViewModel.SelectedItem = param.Selection;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void MapButton_OnClick(object sender, RoutedEventArgs e)
        {
            Debug.Assert(ViewModel.SelectedItem != null);

            Frame.Navigate(typeof (LocationPage), ViewModel.SelectedItem.Location);
        }
    }
}