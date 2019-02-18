using System.Collections.Generic;
using System.Linq;
using Ags.Data.Domain.Media;
using Ags.Services.Media;
using Ags.Web.Models.Media;

namespace Ags.Web.Factories
{
    public class VideoFactory:IVideoFactory
    {
        private readonly IVideoService _videoService;
        private readonly IPictureService _pictureService;

        public VideoFactory(IVideoService videoService, IPictureService pictureService)
        {
            _videoService = videoService;
            _pictureService = pictureService;
        }

        public VideoGalleryModel PrepareVideoGalleryModel(int videoId)
        {
            if(videoId==0)
                return new VideoGalleryModel();
            var query = _videoService.GetByVideoGallery(videoId);
            if(query==null)
                return new VideoGalleryModel();
            var model = new VideoGalleryModel
            {
                Id = query.Id,
                Name = query.Name,
                Published = query.Published,
            };
            var video = _videoService.GetByVideo(videoId);
            model.VideoModels = PrePareVideoGalleryList(videoId);
            return model;
        }

        public List<VideoModel> PrePareVideoGalleryList(int videoId)
        {
            if(videoId==0)
                return new List<VideoModel>();
            var query = _videoService.GetVideoList(videoId);
            var model = new List<VideoModel>();
            foreach (var item in query)
            {
                model.Add(PrepareVideoModel(new VideoModel(), item));
            }

            return model;
        }

        public VideoGalleryListModel PrePareVideoGalleryListModel(VideoGalleryPagingFilteringModel command)
        {
            var model = new VideoGalleryListModel();
            if (command.PageSize <= 0)
            {
                command.PageSize = 15;
            }

            if (command.PageNumber <= 0)
            {
                command.PageNumber = 1;
            }
            var videos = _videoService.GetAllVideoGallery(pageIndex: command.PageNumber, pageSize: command.PageSize);
            model.VideoGalleryModels = videos.Select(x =>
            {
                var galery = new VideoGalleryModel();
                galery.Name = x.Name;
                galery.Published = x.Published;
                galery.Id = x.Id;
                galery.VideoModels = PrePareVideoGalleryList(x.Id);
                return galery;
            }).ToList();
            return model;
        }

        public VideoGalleryPagingFilteringModel PrepareVideoGalleryPagingFilteringModel(int? filterItem)
        {
            throw new System.NotImplementedException();
        }

        public VideoModel PrepareVideoModel(VideoModel model,Video video)
        {
            model.Id = video.Id;
            model.Description = video.Descriptions;
            model.EmbedCode = video.EmbedCode;
            model.DisplayOrder = video.DisplayOrder;
            model.IsApproved = video.IsApproved;
            model.VideoGalleryId = video.VideoGalleryId;
            model.Published = video.Published;
            model.PictureId = video.PictureId;
            model.PictureUrl = video.PictureUrl;
            if (video.PictureId > 0)
            {
                model.ResimUrl = _pictureService.GetPictureUrl(video.PictureId);
            }
            return model;
        }
    }
}