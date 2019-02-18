using System.Collections.Generic;
using System.Linq;
using Ags.Data.Domain;
using Ags.Services.Logging;
using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Areas.Admin.Models.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public partial class LogController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILogger _logger;
        private readonly ILogModelFactory _logModelFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion

        #region Ctor

        public LogController(ICustomerActivityService customerActivityService,

            ILogger logger,
            ILogModelFactory logModelFactory, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager)
        {
            this._customerActivityService = customerActivityService;
            this._logger = logger;
            this._logModelFactory = logModelFactory;
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
            LogSearchModel model = _logModelFactory.PrepareLogSearchModel(new LogSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult LogList(LogSearchModel searchModel)
        {
            //prepare model
            LogListModel model = _logModelFactory.PrepareLogListModel(searchModel);

            return Json(model);
        }

        [HttpPost, ActionName("List")]
        public virtual IActionResult ClearAll()
        {


            _logger.ClearLog();

            //activity log
            _customerActivityService.InsertActivity("DeleteSystemLog", "DeleteSystemLog");

            SuccessNotification("Cleared");

            return RedirectToAction("List");
        }

        public virtual IActionResult View(int id)
        {

            //try to get a log with the specified id
            var log = _logger.GetLogById(id);
            if (log == null)
                return RedirectToAction("List");

            //prepare model
            LogModel model = _logModelFactory.PrepareLogModel(null, log);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {

            //try to get a log with the specified id
           var log = _logger.GetLogById(id);
            if (log == null)
                return RedirectToAction("List");

            _logger.DeleteLog(log);

            //activity log
            _customerActivityService.InsertActivity("DeleteSystemLog", "DeleteSystemLog", log);

            SuccessNotification("Deleted");

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {

            if (selectedIds != null)
                _logger.DeleteLogs(_logger.GetLogByIds(selectedIds.ToArray()).ToList());

            //activity log
            _customerActivityService.InsertActivity("DeleteSystemLog", "DeleteSystemLog");

            return Json(new { Result = true });
        }

        #endregion
    }
}