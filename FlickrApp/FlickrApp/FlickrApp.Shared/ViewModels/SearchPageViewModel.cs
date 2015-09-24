namespace FlickrApp.ViewModels
{
    #region Imports

    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Common;
    using Contracts;
    using Contracts.Events;
    using Providers;

    #endregion

    public class PeriodicTask
    {
        public static async Task Run(Action action, TimeSpan period, CancellationToken cancellationToken)
        {
            do
            {
                action();
                await Task.Delay(period, cancellationToken)
                    .ContinueWith(task =>
                    {
                        /* empty continuation will prevent exception from propagating */
                    });
            } while (! cancellationToken.IsCancellationRequested);
        }

        public static Task Run(Action action, TimeSpan period)
        {
            return Run(action, period, CancellationToken.None);
        }
    }

    public class SearchPageViewModel
        : BaseViewModel
    {
        private readonly TimeSpan TaskPeriod = new TimeSpan(0, 0, 0, 0, 20 /* ms */);

        private readonly IMessengerService _messengerService;
        private uint _searchRequestId;
        private string _theSearchTerm;
        private Task _consumePhotoTask;
        private CancellationTokenSource _cancelTokenSource;
        private readonly PhotoDownloadQueue _photoQueue = PhotoDownloadQueue.Instance;

        public SearchPageViewModel()
        {
            _messengerService = Resolver.Instance.Resolve<IMessengerService>();

            SearchCommand = new RelayCommand(OnSearch, CanExecuteSearch);
            ToggleSearchBarCommand = new RelayCommand(OnToggleSearchBar);
            PauseSearchCommand = new RelayCommand(OnPauseSearch, CanPauseSearch);
            ResumeSearchCommand = new RelayCommand(OnResumeSearch, CanResumeSearch);
        }

        #region Properties

        public bool IsAppBarMinimised { get; set; }

        public bool IsSearchInProgress { get; private set; }

        public string SearchTerm { get; set; }

        public bool IsSearchBarVisible { get; private set; }

        public ObservableCollection<PhotoViewModel> Photos { get; } = new ObservableCollection<PhotoViewModel>();

        #endregion

        #region Commands

        public ICommand SearchCommand { get; }

        public ICommand ToggleSearchBarCommand { get; }

        public ICommand PauseSearchCommand { get; }

        public ICommand ResumeSearchCommand { get; }

        #endregion

        private bool CanExecuteSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                SearchTerm = string.Empty;
                return false;
            }

            return true;
        }

        private void OnSearch()
        {
            Debug.Assert(! string.IsNullOrEmpty(SearchTerm));

            if (! string.Equals(_theSearchTerm, SearchTerm))
            {
                // this is a brand new search...

                Photos.Clear();

                _theSearchTerm = SearchTerm;
                ++_searchRequestId;
            }

            IsSearchInProgress = true;

            _messengerService.Notify(new SearchPhotoEvent(this, _theSearchTerm, _searchRequestId));

            if (_consumePhotoTask == null)
            {
                _cancelTokenSource = new CancellationTokenSource();
                _consumePhotoTask = PeriodicTask.Run(UpdatePhotoCollection, TaskPeriod, _cancelTokenSource.Token);
            }
        }

        private void OnSearchBarShown()
        {
            IsAppBarMinimised = true;
        }

        private void OnSearchBarHidden()
        {
            StopSearch();

            IsAppBarMinimised = false;
        }

        private async void StopSearch()
        {
            _messengerService.Notify(new SearchPhotoEndEvent(this));

            _cancelTokenSource.Cancel();
            await Task.Factory.StartNew(() => { _consumePhotoTask.Wait(); });

            _consumePhotoTask = null;
            _cancelTokenSource = null;

            IsSearchInProgress = false;
        }

        private void OnToggleSearchBar()
        {
            IsSearchBarVisible = ! IsSearchBarVisible;

            if (IsSearchBarVisible)
                OnSearchBarShown();
            else
                OnSearchBarHidden();
        }

        private bool CanPauseSearch()
        {
            return IsSearchInProgress;
        }

        private void OnPauseSearch()
        {
            Debug.Assert(IsSearchInProgress);

            StopSearch();
        }

        private bool CanResumeSearch()
        {
            return (! IsSearchInProgress) && (_theSearchTerm != null);
        }

        private void OnResumeSearch()
        {
            Debug.Assert(! IsSearchInProgress);

            OnSearch();
        }

        private void UpdatePhotoCollection()
        {
            var vm = _photoQueue.Get();

            if (vm?.SearchRequestId == _searchRequestId)
                Photos.Add(vm);
        }
    }
}