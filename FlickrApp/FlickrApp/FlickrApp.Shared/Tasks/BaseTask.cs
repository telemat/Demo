namespace FlickrApp.Tasks
{
    #region Imports

    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    internal abstract class BaseTask
        : IDisposable
    {
        private bool _isDisposed;
        private readonly string _taskName;
        private readonly uint _timeoutInMs;
        private readonly Task _task;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ManualResetEventSlim _runEvent;
        private readonly ManualResetEventSlim _pauseEvent;
        private readonly AutoResetEvent _resumeEvent;        

        protected BaseTask(string taskName, uint timeoutInMs = 1000)
        {
            _taskName = taskName;
            _timeoutInMs = timeoutInMs;
            _cancellationTokenSource = new CancellationTokenSource();
            _runEvent = new ManualResetEventSlim(false);
            _pauseEvent = new ManualResetEventSlim(false);
            _resumeEvent = new AutoResetEvent(false);

            _task = new Task(Execute, _cancellationTokenSource.Token);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool IsRunning => _runEvent.IsSet;

        public void Start()
        {
            _task.Start();

            Debug.WriteLine(_taskName + " task started");
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _task.Wait();

            Debug.WriteLine(_taskName + " task stopped");
        }

        public void Pause()
        {
            _pauseEvent.Set();

            Debug.WriteLine(_taskName + " task paused");
        }

        public void Resume()
        {
            _pauseEvent.Reset();
            _resumeEvent.Set();

            Debug.WriteLine(_taskName + " task resumed");
        }

        private void Execute()
        {
            var cancelToken = _cancellationTokenSource.Token;
            var waitHandles = new[] {_resumeEvent, cancelToken.WaitHandle};

            try
            {
                while (true)
                {
                    var index = WaitHandle.WaitAny(waitHandles, (int) _timeoutInMs);

                    if (index == WaitHandle.WaitTimeout || index == 0 /* wakeup event */)
                        if (! _pauseEvent.IsSet)
                        {
                            _runEvent.Set();

                            DoUsefulWork();

                            _runEvent.Reset();
                        }

                    if (cancelToken.IsCancellationRequested)
                        break;
                }
            }
            catch (AggregateException ex)
            {
                Debug.WriteLine("Task {0} failed, {1}", _taskName, ex.Flatten().Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Task {0} failed, {1}", _taskName, ex.Message);
            }
        }

        protected abstract void DoUsefulWork();

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (! _task.IsCanceled && ! _task.IsCompleted)
                Stop();

            // cleanup
            _cancellationTokenSource.Dispose();
            _resumeEvent.Dispose();

            Debug.WriteLine(_taskName + " task disposed");
        }
    }
}