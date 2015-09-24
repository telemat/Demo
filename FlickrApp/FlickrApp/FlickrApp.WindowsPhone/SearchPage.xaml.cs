// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace FlickrApp
{
    #region Imports

    using System;
    using System.Diagnostics;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Navigation;
    using Common;
    using ViewModels;

    #endregion

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage
        : Page
    {
        public SearchPageViewModel ViewModel { get; }

        public SearchPage()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;            

            ViewModel = DataContext as SearchPageViewModel;
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
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void SearchTextBox_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (ViewModel.SearchCommand.CanExecute(null))
                {
                    ViewModel.SearchCommand.Execute(null);

                    // hide keyboard
                    RemoveFocusOnTextbox(SearchTextBox);
                }

                e.Handled = true;
            }
        }

        private void RemoveFocusOnTextbox(TextBox textBox)
        {
            // this function is just a hack to get the keyboard to hide by itself
            var isTabStop = textBox.IsTabStop;
            textBox.IsTabStop = false;
            textBox.IsEnabled = false;
            textBox.IsEnabled = true;
            textBox.IsTabStop = isTabStop;
        }

        private void Image_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (ViewModel.ThumbnailTappedCommand.CanExecute(null))
            {
                ViewModel.ThumbnailTappedCommand.Execute(null);

                Frame.Navigate(typeof(PivotPage));
            }

            e.Handled = true;
        }

        private void GridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.Assert(e.ClickedItem != null);

            if (ViewModel.ThumbnailTappedCommand.CanExecute(null))
            {
                Frame.Navigate(typeof(PivotPage), e.ClickedItem);
            }
        }

        private void AppBarSearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            // logic is reversed because event is triggered before viewmodel is updated
            var isSearchBarVisible = ! ViewModel.IsSearchBarVisible;

            if (isSearchBarVisible)
            {
                // show the keyboard
                SearchTextBox.Focus(FocusState.Programmatic);

                SearchTextBox.SelectAll();

                // shrink the gridview
                if (! double.IsNaN(_searchBarGridHeight))
                    GridView.Margin = new Thickness(0, 50, 0, 0);
            }
            else
            {
                // hide keyboard
                RemoveFocusOnTextbox(SearchTextBox);

                // expand the gridview
                GridView.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        private double _searchBarGridHeight = Double.NaN;

        private void SearchBarGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Height > e.PreviousSize.Height)
            {
                _searchBarGridHeight = e.NewSize.Height - e.PreviousSize.Height;

                // the bar is expanding, we must shrink the gridview
                GridView.Margin = new Thickness(0, _searchBarGridHeight, 0, 0);
            }
        }

        // TODO - adjust the image widths
        //private void GridViewItem_OnLoaded(object sender, RoutedEventArgs e)
        //{
        //    var element = sender as Grid;
        //    element.Width = gridView.Width / 2;
        //    element.Height = element.Width;
        //}
    }
}