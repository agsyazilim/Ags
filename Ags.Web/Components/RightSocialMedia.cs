using Ags.Services.Common;
using Ags.Services.Media;
using Ags.Web.Framework.Components;
using Ags.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class RightSocialMediaViewComponent : AgsViewComponent
    {
        private readonly IPictureService _pictureService;
        private readonly ISectionService _sectionService;
        private readonly IAdvertisementService _advertisementService;

        public RightSocialMediaViewComponent(IPictureService pictureService, IAdvertisementService advertisementService, ISectionService sectionService)
        {
            this._pictureService = pictureService;
            _advertisementService = advertisementService;
            _sectionService = sectionService;

        }

        public IViewComponentResult Invoke()
        {
            var section = _sectionService.GetByName("RightSocialMedia");
            if (section == null)
                return Content("");
            var adv = _advertisementService.GetBySectionId(section.Id);
            var model = new AdvertisementModel
            {
                Id = adv.Id,
                PictureUrl = _pictureService.GetPictureUrl(adv.PictureId),
                UrlAddress = adv.UrlAddress,
                CodeFlash = adv.CodeFlash,
                FlashCode = adv.FlashCode,
                Target = adv.TargetId == true ? "_parent" : ""
            };
            return View(model);

        }
    }
}