using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Media
{
    public class PhotoGalleryListModel : BaseAgsModel
    {
        public PhotoGalleryListModel()
        {
            GalleryModels = new List<GalleryModel>();
            PhotoGalleryPagingFilteringModel = new PhotoGalleryPagingFilteringModel();
            PhotoGalleryModels = new List<PhotoGalleryModel>();

        }

        public PhotoGalleryPagingFilteringModel PhotoGalleryPagingFilteringModel { get; set; }
        public IList<PhotoGalleryModel> PhotoGalleryModels { get; set; }
        public IList<GalleryModel> GalleryModels { get; set; }
    }
}