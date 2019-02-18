using System.Collections.Generic;
using System.Linq;
using Ags.Services.Media;
using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Ags.Web.Models.Media;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class GalleryViewComponent:AgsViewComponent
    {
        private readonly IGalleryFactory _galleryFactory;
        private readonly IGalleryService _galleryService;

        public GalleryViewComponent(IGalleryFactory galleryFactory, IGalleryService galleryService)
        {
            _galleryFactory = galleryFactory;
            _galleryService = galleryService;
        }

        public IViewComponentResult Invoke()
        {

            var model = new List<PhotoGalleryModel>();
            var galleryList = _galleryService.GetGalleryMappingList().Where(x=>x.Gallery.Name!= "InstagtamGaleri");
            foreach (var gallery in galleryList)
            {
                model.Add(_galleryFactory.PreParePhotoGalleryModel(new PhotoGalleryModel(), gallery));
            }
            return View(model);
        }
    }
}