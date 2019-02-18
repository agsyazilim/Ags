using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Media
{
    public class VideoGalleryModel : BaseAgsEntityModel
    {
        public VideoGalleryModel()
        {
            VideoModels = new List<VideoModel>();
        }

        public string Name { get; set; }

        public bool Published { get; set; }

        public List<VideoModel> VideoModels { get; set; }


    }
}