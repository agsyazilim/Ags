using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core;
using Ags.Data.Core.Caching;
using Ags.Services.Seo;
using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Areas.Admin.Models.Common;
using Ags.Web.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public partial class CommonController : BaseAdminController
    {
        #region Fields

        private readonly ICommonModelFactory _commonModelFactory;
        private readonly IAgsFileProvider _fileProvider;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public CommonController(ICommonModelFactory commonModelFactory,
            IAgsFileProvider fileProvider,
            IStaticCacheManager cacheManager,
            IUrlRecordService urlRecordService,
            IWebHelper webHelper)
        {
            this._commonModelFactory = commonModelFactory;
            this._fileProvider = fileProvider;
            this._cacheManager = cacheManager;
            this._urlRecordService = urlRecordService;
            this._webHelper = webHelper;
        }

        #endregion

        #region Methods

        [HttpPost]
        public virtual IActionResult ClearCache(string returnUrl = "")
        {
            _cacheManager.Clear();
            //home page
            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home", new { area = AreaNames.Admin });
            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home", new { area = AreaNames.Admin });
            return Redirect(returnUrl);
        }



        public virtual IActionResult SeNames()
        {


            //prepare model
            UrlRecordSearchModel model = _commonModelFactory.PrepareUrlRecordSearchModel(new UrlRecordSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult RestartApplication(string returnUrl = "")
        {

            //restart application
            _webHelper.RestartAppDomain();
            //home page
            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home", new {area = AreaNames.Admin});
            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home", new {area = AreaNames.Admin});
            return Redirect(returnUrl);
        }
        [HttpPost]
        public virtual IActionResult SeNames(UrlRecordSearchModel searchModel)
        {
            //prepare model
            UrlRecordListModel model = _commonModelFactory.PrepareUrlRecordListModel(searchModel);
            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult DeleteSelectedSeNames(ICollection<int> selectedIds)
        {if (selectedIds != null)
                _urlRecordService.DeleteUrlRecords(_urlRecordService.GetUrlRecordsByIds(selectedIds.ToArray()));

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult PopularSearchTermsReport(PopularSearchTermSearchModel searchModel)
        {
            //prepare model
            PopularSearchTermListModel model = _commonModelFactory.PreparePopularSearchTermListModel(searchModel);
            return Json(model);
        }

        //action displaying notification (warning) to a store owner that entered SE URL already exists
        public virtual IActionResult UrlReservedWarning(string entityId, string entityName, string seName)
        {
            if (string.IsNullOrEmpty(seName))
                return Json(new { Result = string.Empty });

            int.TryParse(entityId, out int parsedEntityId);
            string validatedSeName = _urlRecordService.ValidateSeName(parsedEntityId, entityName, seName, null, false);

            if (seName.Equals(validatedSeName, StringComparison.InvariantCultureIgnoreCase))
                return Json(new { Result = string.Empty });

            return Json(new { Result = string.Format("Reserved{0}", validatedSeName) });
        }

        #endregion
    }
}