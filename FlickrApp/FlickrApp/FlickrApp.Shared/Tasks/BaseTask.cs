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
        private readonly AutoResetEvent _wakeupEvent;

        protected BaseTask(string taskName, uint timeoutInMs = 1000)
        {
            _taskName = taskName;
            _timeoutInMs = timeoutInMs;
            _cancellationTokenSource = new CancellationTokenSource();
            _wakeupEvent = new AutoResetEvent(false);

            _task = new Task(Execute, _cancellationTokenSource.Token);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);            
        }

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

        private void Execute()
        {
            var cancelToken = _cancellationTokenSource.Token;
            var waitHandles = new[] {_wakeupEvent, cancelToken.WaitHandle};

            try
            {
                while (true)
                {
                    var index = WaitHandle.WaitAny(waitHandles, (int) _timeoutInMs);

                    if (index == WaitHandle.WaitTimeout || index == 0 /* wakeup event */)
                        DoUsefulWork();

                    if (cancelToken.IsCancellationRequested)
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Task {0} failed, {1}", _taskName, ex.Message);
            }
        }

        protected void WakeUp()
        {
            _wakeupEvent.Set();
        }

        protected abstract void DoUsefulWork();

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            if (!_task.IsCanceled && !_task.IsCompleted)
                Stop();

            // cleanup
            _cancellationTokenSource.Dispose();
            _wakeupEvent.Dispose();

            Debug.WriteLine(_taskName + " task disposed");            
        }
    }
}