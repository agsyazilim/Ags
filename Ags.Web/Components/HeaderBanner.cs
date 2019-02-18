using System;
using Ags.Data.Domain;
using Ags.Services.Configuration;
using Ags.Services.Media;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class HeaderBanner : AgsViewComponent
    {
        private readonly ISettingService _settingService;
        private readonly IPictureService _pictureService;
        public HeaderBanner(ISettingService settingService, IPictureService pictureService)
        {
            _settingService = settingService;
            _pictureService = pictureService;
        }

        public IViewComponentResult Invoke()
        {
           var storeInformationSettings = _settingService.LoadSetting<StoreInformationSettings>();
            int pictureId = storeInformationSettings.HeaderBannerPictureId;
            string logoUrl = _pictureService.GetPictureUrl(pictureId);
            Tuple<string> message;
            message = Tuple.Create(logoUrl);
            return View(message);
        }
    }
}