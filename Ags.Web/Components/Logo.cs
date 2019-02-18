using System.Linq;
using Ags.Data.Domain;
using Ags.Services.Configuration;
using Ags.Services.Media;
using Ags.Services.Stores;
using Ags.Web.Areas.Admin.Models;
using Ags.Web.Framework.Components;
using Ags.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class LogoViewComponent : AgsViewComponent
    {
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;

        public LogoViewComponent(IPictureService pictureService,ISettingService settingService, IStoreService storeService)
        {
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._storeService = storeService;
        }

        public IViewComponentResult Invoke()
        {
            StoreInformationSettings storeInformationSettings = _settingService.LoadSetting<StoreInformationSettings>();
            int pictureId = storeInformationSettings.LogoPictureId;
            string logoUrl = _pictureService.GetPictureUrl(pictureId);
            var model = new LogoModel
            {
                StoreName = _storeService.GetAllStores(false).FirstOrDefault()?.Name,
                LogoPath = logoUrl
            };
            ViewBag.PageHelpContainer = logoUrl;
            return View(model);
        }
    }
}