using System.Collections.Generic;
using Ags.Data.Domain.Media;
using Ags.Web.Models.Media;

namespace Ags.Web.Factories
{
    public interface IVideoFactory
    {
        VideoGalleryModel PrepareVideoGalleryModel(int videoId);
        VideoModel PrepareVideoModel(VideoModel model,Video video);
        List<VideoModel> PrePareVideoGalleryList(int videoId);
        VideoGalleryListModel PrePareVideoGalleryListModel(VideoGalleryPagingFilteringModel command);
        VideoGalleryPagingFilteringModel PrepareVideoGalleryPagingFilteringModel(int? filterItem);


    }
}