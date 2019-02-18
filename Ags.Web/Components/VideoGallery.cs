using System.Collections.Generic;
using Ags.Services.Media;
using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Ags.Web.Models.Media;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class VideoGalleryViewComponent:AgsViewComponent
    {
        private readonly IVideoFactory _videoFactory;
        private readonly IVideoService _videoService;

        public VideoGalleryViewComponent(IVideoFactory videoFactory, IVideoService videoService)
        {
            _videoFactory = videoFactory;
            _videoService = videoService;
        }

        public IViewComponentResult Invoke()
        {

            var videos = _videoService.GetVideoList();
            var model = new List<VideoModel>();
            foreach (var video in videos)
            {
                model.Add(_videoFactory.PrepareVideoModel(new VideoModel(), video));
            }
            return View(model);
        }
    }
}