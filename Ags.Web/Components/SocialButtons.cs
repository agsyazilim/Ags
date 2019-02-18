using Ags.Data.Domain;
using Ags.Services.Configuration;
using Ags.Web.Framework.Components;
using Ags.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class SocialButtonsViewComponent: AgsViewComponent
    {
        private readonly ISettingService _settingService;
        public SocialButtonsViewComponent(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public IViewComponentResult Invoke()
        {
            StoreInformationSettings storeInformationSettings = _settingService.LoadSetting<StoreInformationSettings>();

            SocialModel model = new SocialModel
            {
                FacebookLink = storeInformationSettings.FacebookLink,
                GooglePlusLink = storeInformationSettings.GooglePlusLink,
                TwitterLink = storeInformationSettings.TwitterLink,
                YoutubeLink = storeInformationSettings.YoutubeLink,
                Instagram = storeInformationSettings.InstagramLink
            };

            return View(model);
        }
    }
}