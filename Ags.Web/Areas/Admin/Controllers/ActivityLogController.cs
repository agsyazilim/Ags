using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Services.Logging;
using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Areas.Admin.Models.Logging;
using Ags.Web.Framework.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public partial class ActivityLogController : BaseAdminController
    {
        #region Fields

        private readonly IActivityLogModelFactory _activityLogModelFactory;
        private readonly ICustomerActivityService _customerActivityService;

        #endregion

        #region Ctor

        public ActivityLogController(IActivityLogModelFactory activityLogModelFactory,
            ICustomerActivityService customerActivityService
            )
        {
            this._activityLogModelFactory = activityLogModelFactory;
            this._customerActivityService = customerActivityService;
        }

        #endregion

        #region Methods

        public virtual  IActionResult List()
        {
            //prepare model
            ActivityLogContainerModel model = _activityLogModelFactory.PrepareActivityLogContainerModel(new ActivityLogContainerModel());
            return View(model);
        }

        public virtual IActionResult ListTypes()
        {
            //prepare model
            System.Collections.Generic.IList<ActivityLogTypeModel> model = _activityLogModelFactory.PrepareActivityLogTypeModels();
            return View(model);
        }

        [HttpPost, ActionName("ListTypes")]
        public virtual IActionResult SaveTypes(IFormCollection form)
        {
            //activity log
            _customerActivityService.InsertActivity("EditActivityLogTypes", "EditActivityLogTypes");
            //get identifiers of selected activity types
            List<int> selectedActivityTypesIds = form["checkbox_activity_types"]
                .SelectMany(value => value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(idString => int.TryParse(idString, out int id) ? id : 0)
                .Distinct().ToList();

            //update activity types
           var activityTypes = _customerActivityService.GetAllActivityTypes();
            foreach (var activityType in activityTypes)
            {
                activityType.Enabled = selectedActivityTypesIds.Contains(activityType.Id);
                _customerActivityService.UpdateActivityType(activityType);
            }
            SuccessNotification("Updated");
            //selected tab
            SaveSelectedTabName();
            return RedirectToAction("List");
        }

        public virtual IActionResult ListLogs()
        {
            //prepare model
            ActivityLogSearchModel model = _activityLogModelFactory.PrepareActivityLogSearchModel(new ActivityLogSearchModel());
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ListLogs(ActivityLogSearchModel searchModel)
        {
            //prepare model
            ActivityLogListModel model = _activityLogModelFactory.PrepareActivityLogListModel(searchModel);
            return Json(model);
        }

        public virtual IActionResult ActivityLogDelete(int id)
        {
            //try to get a log item with the specified id
           var logItem = _customerActivityService.GetActivityById(id)
                ?? throw new ArgumentException("No activity log found with the specified id", nameof(id));
            _customerActivityService.DeleteActivity(logItem);
            //activity log
            _customerActivityService.InsertActivity("DeleteActivityLog", "DeleteActivityLog", logItem);
            return new NullJsonResult();
        }

        public virtual IActionResult ClearAll()
        {
            _customerActivityService.ClearAllActivities();
            //activity log
            _customerActivityService.InsertActivity("DeleteActivityLog", "DeleteActivityLog");
            return RedirectToAction("List");
        }

        #endregion
    }
}