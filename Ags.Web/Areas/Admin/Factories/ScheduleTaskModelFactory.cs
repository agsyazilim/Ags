using System;
using System.Linq;
using Ags.Services.Tasks;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Tasks;
using Ags.Web.Framework.Extensions;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the schedule task model factory implementation
    /// </summary>
    public partial class ScheduleTaskModelFactory : IScheduleTaskModelFactory
    {
        #region Fields

        private readonly IScheduleTaskService _scheduleTaskService;

        #endregion

        #region Ctor

        public ScheduleTaskModelFactory(
            IScheduleTaskService scheduleTaskService)
        {
            this._scheduleTaskService = scheduleTaskService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare schedule task search model
        /// </summary>
        /// <param name="searchModel">Schedule task search model</param>
        /// <returns>Schedule task search model</returns>
        public virtual ScheduleTaskSearchModel PrepareScheduleTaskSearchModel(ScheduleTaskSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged schedule task list model
        /// </summary>
        /// <param name="searchModel">Schedule task search model</param>
        /// <returns>Schedule task list model</returns>
        public virtual ScheduleTaskListModel PrepareScheduleTaskListModel(ScheduleTaskSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get schedule tasks
          var scheduleTasks = _scheduleTaskService.GetAllTasks(true);

            //prepare list model
            ScheduleTaskListModel model = new ScheduleTaskListModel
            {
                Data = scheduleTasks.PaginationByRequestModel(searchModel).Select(scheduleTask =>
                {
                    //fill in model values from the entity
                    ScheduleTaskModel scheduleTaskModel = scheduleTask.ToModel<ScheduleTaskModel>();

                    //convert dates to the user time
                    if (scheduleTask.LastStartUtc.HasValue)
                    {
                        scheduleTaskModel.LastStartUtc = scheduleTask.LastStartUtc.Value.ToString("G");
                    }

                    if (scheduleTask.LastEndUtc.HasValue)
                    {
                        scheduleTaskModel.LastEndUtc = scheduleTask.LastEndUtc.Value.ToString("G");
                    }

                    if (scheduleTask.LastSuccessUtc.HasValue)
                    {
                        scheduleTaskModel.LastSuccessUtc = scheduleTask.LastSuccessUtc.Value.ToString("G");
                    }

                    return scheduleTaskModel;
                }),
                Total = scheduleTasks.Count
            };

            return model;
        }

        #endregion
    }
}