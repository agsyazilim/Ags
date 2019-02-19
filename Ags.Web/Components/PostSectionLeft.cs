using System.Collections.Generic;
using Ags.Services.Common;
using Ags.Services.Media;
using Ags.Web.Framework.Components;
using Ags.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class PostSectionLeftViewComponent : AgsViewComponent
    {
        private readonly IPictureService _pictureService;
        private readonly ISectionService _sectionService;
        private readonly IAdvertisementService _advertisementService;

        public PostSectionLeftViewComponent(IPictureService pictureService, ISectionService sectionService, IAdvertisementService advertisementService)
        {
            this._pictureService = pictureService;
            _sectionService = sectionService;
            _advertisementService = advertisementService;

        }

        public IViewComponentResult Invoke()
        {
            var section = _sectionService.GetByName("PostSectionLeft");
            if (section == null)
                return Content("");
            var adv = _advertisementService.GetBySectionId(section.Id);
            if (adv == null)
                return Content("");
            var model = new AdvertisementModel
            {
                Id = adv.Id,
                PictureUrl = _pictureService.GetPictureUrl(adv.PictureId),
                UrlAddress = adv.UrlAddress,
                CodeFlash = adv.CodeFlash,
                FlashCode = adv.FlashCode,
                Target = adv.TargetId==true?"_parent":"",
                IsApproved = adv.IsApproved
            };
            return View(model);
        }
    }
}