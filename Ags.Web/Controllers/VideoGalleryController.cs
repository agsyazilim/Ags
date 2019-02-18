using Ags.Services.Media;
using Ags.Web.Factories;
using Ags.Web.Models.Media;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Controllers
{
    public class VideoGalleryController : BasePublicController
    {
        private IVideoFactory _videoFactory;
        private IVideoService _videoService;

        public VideoGalleryController(IVideoFactory videoFactory, IVideoService videoService)
        {
            _videoFactory = videoFactory;
            _videoService = videoService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            var model = _videoFactory.PrePareVideoGalleryListModel(new VideoGalleryPagingFilteringModel());
            return View(model);
        }

        public IActionResult VideoDetail(int id)
        {
            var video = _videoService.GetByVideo(id);
            var model = _videoFactory.PrepareVideoGalleryModel(video.Id);
            return View(model);


        }
    }
}