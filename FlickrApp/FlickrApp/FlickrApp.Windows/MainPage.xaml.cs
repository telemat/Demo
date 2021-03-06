﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FlickrApp
{
    using System.Diagnostics;
    using Contracts;
    using Contracts.Events;
    using Contracts.Models;
    using ViewModels;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            //this.DataContext = new MainPageViewModel();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        async private void AuthenticateBtn_Click(object sender, RoutedEventArgs e)
        {
            //Resolver.Instance.Resolve<IMessengerService>().Register<AuthenticationEvent>(it =>
            //{
            //    if (it.IsSuccessful)
            //        Frame.Navigate(typeof(SearchPage));

            //    Resolver.Instance.Resolve<IMessengerService>().Unregister<AuthenticationEvent>();
            //});

            var flickrService = Resolver.Instance.Resolve<IFlickrService>();

            try
            {
                var isSuccess = flickrService.Initialize("c4e9f03344dc58da787a58c8aaf9b9b5", "");
                var result =
                    await
                        flickrService.SearchAsync(new PhotoSearchOption()
                        {
                            PageNumber = 1,
                            PageSize = 10,
                            SearchTerm = "Mauritius"
                        });

                Debug.WriteLine(result.Count);

                //messengerService.Notify(new AuthenticationEvent(isSuccess));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LocationPage));
        }
    }
}
