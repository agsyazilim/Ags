using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Media
{
    public class VideoGalleryListModel: BaseAgsModel
    {
        public VideoGalleryListModel()
        {
            VideoGalleryModels = new List<VideoGalleryModel>();
            VideoGalleryPagingFilteringModel = new VideoGalleryPagingFilteringModel();
            VideoModels = new List<VideoModel>();
        }
        public VideoGalleryPagingFilteringModel VideoGalleryPagingFilteringModel { get; set; }
        public IList<VideoGalleryModel> VideoGalleryModels { get; set; }
        public IList<VideoModel> VideoModels { get; set; }
    }
}