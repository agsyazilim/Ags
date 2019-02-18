using System;
using System.Collections.Generic;
using System.Linq;
using AdminLTE.Areas.Admin.Factories;
using Ags.Services.Logging;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Logging;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the activity log model factory implementation
    /// </summary>
    public partial class ActivityLogModelFactory : IActivityLogModelFactory
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;

        #endregion

        #region Ctor

        public ActivityLogModelFactory(
            ICustomerActivityService customerActivityService, IBaseAdminModelFactory baseAdminModelFactory)
        {
            this._customerActivityService = customerActivityService;
            _baseAdminModelFactory = baseAdminModelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare activity log container model
        /// </summary>
        /// <param name="activityLogContainerModel">Activity log container model</param>
        /// <returns>Activity log container model</returns>
        public virtual ActivityLogContainerModel PrepareActivityLogContainerModel(ActivityLogContainerModel activityLogContainerModel)
        {
            if (activityLogContainerModel == null)
                throw new ArgumentNullException(nameof(activityLogContainerModel));

            //prepare nested models
            PrepareActivityLogSearchModel(activityLogContainerModel.ListLogs);
            activityLogContainerModel.ListTypes = PrepareActivityLogTypeModels();

            return activityLogContainerModel;
        }

        /// <summary>
        /// Prepare activity log type models
        /// </summary>
        /// <returns>List of activity log type models</returns>
        public virtual IList<ActivityLogTypeModel> PrepareActivityLogTypeModels()
        {
            //prepare available activity log types
            var availableActivityTypes = _customerActivityService.GetAllActivityTypes();
            var models = availableActivityTypes.Select(activityType => activityType.ToModel<ActivityLogTypeModel>()).ToList();

            return models;
        }

        /// <summary>
        /// Prepare activity log search model
        /// </summary>
        /// <param name="searchModel">Activity log search model</param>
        /// <returns>Activity log search model</returns>
        public virtual ActivityLogSearchModel PrepareActivityLogSearchModel(ActivityLogSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare available activity log types
            _baseAdminModelFactory.PrepareActivityLogTypes(searchModel.ActivityLogType);

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged activity log list model
        /// </summary>
        /// <param name="searchModel">Activity log search model</param>
        /// <returns>Activity log list model</returns>
        public virtual ActivityLogListModel PrepareActivityLogListModel(ActivityLogSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get parameters to filter log
            DateTime? startDateValue = searchModel.CreatedOnFrom == null ? null
                : (DateTime?)searchModel.CreatedOnFrom.Value;
            DateTime? endDateValue = searchModel.CreatedOnTo == null ? null
                : (DateTime?)searchModel.CreatedOnTo.Value.AddDays(1);

            //get log
            var activityLog = _customerActivityService.GetAllActivities(createdOnFrom: startDateValue,
                createdOnTo: endDateValue,
                activityLogTypeId: searchModel.ActivityLogTypeId,
                ipAddress: searchModel.IpAddress,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            ActivityLogListModel model = new ActivityLogListModel
            {
                Data = activityLog.Select(logItem =>
                {
                    //fill in model values from the entity
                    ActivityLogModel logItemModel = logItem.ToModel<ActivityLogModel>();

                    //convert dates to the user time
                    logItemModel.CreatedOn =  logItem.CreatedOnUtc;

                    return logItemModel;

                }),
                Total = activityLog.TotalCount
            };

            return model;
        }

        #endregion
    }
}