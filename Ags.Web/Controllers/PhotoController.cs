using Ags.Services.Media;
using Ags.Web.Factories;
using Ags.Web.Models.Media;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Controllers
{
    public class PhotoController : BasePublicController
    {
        private readonly IGalleryFactory _galleryFactory;
        private readonly IGalleryService _galleryService;

        public PhotoController(IGalleryFactory galleryFactory, IGalleryService galleryService)
        {
            _galleryFactory = galleryFactory;
            _galleryService = galleryService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            var model = _galleryFactory.PrePareGalleryListModel(new PhotoGalleryPagingFilteringModel());
            return View(model);
        }

        public IActionResult PhotoDetail(int id)
        {
            var galeri = _galleryService.GetById(id);
            var model = _galleryFactory.PreParePhotoGalleryListModel(galeri.Id);
            return View(model);


        }
    }
}