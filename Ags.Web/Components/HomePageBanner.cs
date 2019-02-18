using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Domain;
using Ags.Services.Common;
using Ags.Services.Media;
using Ags.Web.Framework.Components;
using Ags.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class HomePageBannerViewComponent : AgsViewComponent
    {
        private readonly IPictureService _pictureService;
        private readonly ISectionService _sectionService;
        private readonly IAdvertisementService _advertisementService;

        public HomePageBannerViewComponent(IPictureService pictureService, ISectionService sectionService, IAdvertisementService advertisementService)
        {
            this._pictureService = pictureService;
            _sectionService = sectionService;
            _advertisementService = advertisementService;

        }

        public IViewComponentResult Invoke()
        {
            var section = _sectionService.GetByName("HomePageBanner");
            if (section == null)
                return Content("");
            var adv = _advertisementService.GetBySectionId(section.Id);
            var model = new AdvertisementModel
            {
                Id = adv.Id,
                PictureUrl = _pictureService.GetPictureUrl(adv.PictureId, 500),
                UrlAddress = adv.UrlAddress,
                CodeFlash = adv.CodeFlash,
                FlashCode = adv.FlashCode,
                Target = adv.TargetId == true ? "_parent" : ""
            };
            return View(model);
        }
    }
}