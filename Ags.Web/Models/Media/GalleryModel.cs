using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Media
{
    public class GalleryModel:BaseAgsEntityModel
    {
        public GalleryModel()
        {
            PhotoGalleryModels = new List<PhotoGalleryModel>();
        }
        public string Name { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }
        public List<PhotoGalleryModel> PhotoGalleryModels { get; set; }
    }
}