using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Ags.Data.Core;
using Ags.Data.Domain.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Ags.Services.Tasks
{
    /// <summary>
    /// Represents task manager
    /// </summary>
    public partial class TaskManager
    {
        #region Fields

        private readonly List<TaskThread> _taskThreads = new List<TaskThread>();

        #endregion

        #region Ctor

        private TaskManager()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the task manager
        /// </summary>
        public void Initialize()
        {
            _taskThreads.Clear();


            IScheduleTaskService taskService = ServiceProviderFactory.ServiceProvider.GetService<IScheduleTaskService>();
            List<ScheduleTask> scheduleTasks = taskService
                .GetAllTasks()
                .OrderBy(x => x.Seconds)
                .ToList();

            foreach (ScheduleTask scheduleTask in scheduleTasks)
            {
                TaskThread taskThread = new TaskThread
                {
                    Seconds = scheduleTask.Seconds
                };

                //sometimes a task period could be set to several hours (or even days)
                //in this case a probability that it'll be run is quite small (an application could be restarted)
                //calculate time before start an interrupted task
                if (scheduleTask.LastStartUtc.HasValue)
                {
                    //seconds left since the last start
                    double secondsLeft = (DateTime.UtcNow - scheduleTask.LastStartUtc).Value.TotalSeconds;

                    if (secondsLeft >= scheduleTask.Seconds)
                    {
                        //run now (immediately)
                        taskThread.InitSeconds = 0;
                    }
                    else
                    {
                        //calculate start time
                        //and round it (so "ensureRunOncePerPeriod" parameter was fine)
                        taskThread.InitSeconds = (int)(scheduleTask.Seconds - secondsLeft) + 1;
                    }
                }
                else
                {
                    //first start of a task
                    taskThread.InitSeconds = scheduleTask.Seconds;
                }

                taskThread.AddTask(scheduleTask);
                _taskThreads.Add(taskThread);
            }
        }

        /// <summary>
        /// Starts the task manager
        /// </summary>
        public void Start()
        {
            foreach (TaskThread taskThread in _taskThreads)
            {
                taskThread.InitTimer();
            }
        }

        /// <summary>
        /// Stops the task manager
        /// </summary>
        public void Stop()
        {
            foreach (TaskThread taskThread in _taskThreads)
            {
                taskThread.Dispose();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the task manger instance
        /// </summary>
        public static TaskManager Instance { get; } = new TaskManager();

        /// <summary>
        /// Gets a list of task threads of this task manager
        /// </summary>
        public IList<TaskThread> TaskThreads => new ReadOnlyCollection<TaskThread>(_taskThreads);

        #endregion
    }
}
