using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading;
using AdminLTE.Core;
using Ags.Data.Common;
using Ags.Data.Core;
using Ags.Data.Domain.Tasks;
using Ags.Services.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Ags.Services.Tasks
{
    /// <summary>
    /// Represents task thread
    /// </summary>
    public partial class TaskThread : IDisposable
    {
        #region Fields

        private static readonly string ScheduleTaskUrl;
        private readonly Dictionary<string, string> _tasks;
        private Timer _timer;
        private bool _disposed;

        #endregion

        #region Ctors

        static TaskThread()
        {
            IStoreContext storeContext = ServiceProviderFactory.ServiceProvider.GetService<IStoreContext>();
            ScheduleTaskUrl = $"{storeContext.CurrentStore.Url}{AgsTaskDefaults.ScheduleTaskPath}";
        }

        internal TaskThread()
        {
            this._tasks = new Dictionary<string, string>();
            this.Seconds = 10 * 60;
        }

        #endregion

        #region Utilities

        private void Run()
        {
            if (Seconds <= 0)
                return;

            StartedUtc = DateTime.UtcNow;
            IsRunning = true;
            foreach (string taskType in _tasks.Values)
            {
                //create and send post data
                NameValueCollection postData = new NameValueCollection
                {
                    { "taskType", taskType }
                };

                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.UploadValues(ScheduleTaskUrl, postData);
                    }
                }
                catch (Exception ex)
                {
                    //var serviceScopeFactory = ServiceProviderFactory.ServiceProvider.GetRequiredService<IServiceScopeFactory>();

                    //using (var scope = serviceScopeFactory.CreateScope())
                    //{
                    //    // Resolve
                    //    var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                    //    logger.Error(ex.Message, ex);
                    //}
                    ILogger logger = ServiceProviderFactory.ServiceProvider.GetService<ILogger>();
                    logger.Error(ex.Message, ex);

                }
            }

            IsRunning = false;
        }

        private void TimerHandler(object state)
        {
            _timer.Change(-1, -1);
            Run();
            if (RunOnlyOnce)
            {
                Dispose();
            }
            else
            {
                _timer.Change(Interval, Interval);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Disposes the instance
        /// </summary>
        public void Dispose()
        {
            if (_timer == null || _disposed)
                return;

            lock (this)
            {
                _timer.Dispose();
                _timer = null;
                _disposed = true;
            }
        }

        /// <summary>
        /// Inits a timer
        /// </summary>
        public void InitTimer()
        {
            if (_timer == null)
            {
                _timer = new Timer(TimerHandler, null, InitInterval, Interval);
            }
        }

        /// <summary>
        /// Adds a task to the thread
        /// </summary>
        /// <param name="task">The task to be added</param>
        public void AddTask(ScheduleTask task)
        {
            if (!_tasks.ContainsKey(task.Name))
            {
                _tasks.Add(task.Name, task.Type);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the interval in seconds at which to run the tasks
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// Get or set the interval before timer first start
        /// </summary>
        public int InitSeconds { get; set; }

        /// <summary>
        /// Get or sets a datetime when thread has been started
        /// </summary>
        public DateTime StartedUtc { get; private set; }

        /// <summary>
        /// Get or sets a value indicating whether thread is running
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets the interval (in milliseconds) at which to run the task
        /// </summary>
        public int Interval
        {
            get
            {
                //if somebody entered more than "2147483" seconds, then an exception could be thrown (exceeds int.MaxValue)
                int interval = Seconds * 1000;
                if (interval <= 0)
                    interval = int.MaxValue;
                return interval;
            }
        }

        /// <summary>
        /// Gets the due time interval (in milliseconds) at which to begin start the task
        /// </summary>
        public int InitInterval
        {
            get
            {
                //if somebody entered less than "0" seconds, then an exception could be thrown
                int interval = InitSeconds * 1000;
                if (interval <= 0)
                    interval = 0;
                return interval;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the thread would be run only once (on application start)
        /// </summary>
        public bool RunOnlyOnce { get; set; }

        #endregion
    }
}