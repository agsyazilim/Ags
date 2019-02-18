using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Media
{
    public class PhotoGalleryModel:BaseAgsEntityModel
    {
        public PhotoGalleryModel()
        {
            PictureModels = new PictureModel();
        }
        public int PictureId { get; set; }
        public int GalleryId { get; set; }
        public int DisplayOrder { get; set; }
        public string Url { get; set; }
        public PictureModel PictureModels { get; set; }
         
    }
}