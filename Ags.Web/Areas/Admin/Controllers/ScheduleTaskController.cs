using System;
using Ags.Data.Domain;
using Ags.Services.Logging;
using Ags.Services.Tasks;
using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Areas.Admin.Models.Tasks;
using Ags.Web.Framework.Kendoui;
using Ags.Web.Framework.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Controllers
{
    public partial class ScheduleTaskController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly IScheduleTaskModelFactory _scheduleTaskModelFactory;
        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Ctor

        public ScheduleTaskController(ICustomerActivityService customerActivityService,
            IScheduleTaskModelFactory scheduleTaskModelFactory,
            IScheduleTaskService scheduleTaskService, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager)
        {
            this._customerActivityService = customerActivityService;
            this._scheduleTaskModelFactory = scheduleTaskModelFactory;
            this._scheduleTaskService = scheduleTaskService;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {

            //prepare model
            ScheduleTaskSearchModel model = _scheduleTaskModelFactory.PrepareScheduleTaskSearchModel(new ScheduleTaskSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ScheduleTaskSearchModel searchModel)
        {
            //prepare model
            ScheduleTaskListModel model = _scheduleTaskModelFactory.PrepareScheduleTaskListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult TaskUpdate(ScheduleTaskModel model)
        {


            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });

            //try to get a schedule task with the specified id
          var scheduleTask = _scheduleTaskService.GetTaskById(model.Id)
                               ?? throw new ArgumentException("Schedule task cannot be loaded");

            scheduleTask.Name = model.Name;
            scheduleTask.Seconds = model.Seconds;
            scheduleTask.Enabled = model.Enabled;
            scheduleTask.StopOnError = model.StopOnError;
            _scheduleTaskService.UpdateTask(scheduleTask);

            //activity log
            _customerActivityService.InsertActivity("EditTask",
                string.Format("EditTask{0}", scheduleTask.Id), scheduleTask);

            return new NullJsonResult();
        }

        public virtual IActionResult RunNow(int id)
        {


            try
            {
                //try to get a schedule task with the specified id
                var scheduleTask = _scheduleTaskService.GetTaskById(id)
                                   ?? throw new ArgumentException("Schedule task cannot be loaded", nameof(id));

                //ensure that the task is enabled
                Task task = new Task(scheduleTask) { Enabled = true };
                task.Execute(true, false);

                SuccessNotification("RunNow.Done");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
            }

            return RedirectToAction("List");
        }

        #endregion
    }
}