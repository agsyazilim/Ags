using Ags.Services.Common;
using Ags.Services.Media;
using Ags.Web.Framework.Components;
using Ags.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class RightCategorySocialMediaViewComponent : AgsViewComponent
    {
        private readonly IPictureService _pictureService;
        private readonly ISectionService _sectionService;
        private readonly IAdvertisementService _advertisementService;
        public RightCategorySocialMediaViewComponent(IPictureService pictureService, ISectionService sectionService, IAdvertisementService advertisementService)
        {
            this._pictureService = pictureService;
            _sectionService = sectionService;
            _advertisementService = advertisementService;
        }

        public IViewComponentResult Invoke(string categoryName)
        {
            var section = _sectionService.GetByName(categoryName);
            if(section==null)
                return View(new AdvertisementModel());
            var adv = _advertisementService.GetBySectionId(section.Id);
            if (adv == null)
                return View(new AdvertisementModel());
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