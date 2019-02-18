using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Media
{
    public class VideoModel:BaseAgsEntityModel
    {

        public int VideoGalleryId { get; set; }
        public string Description { get; set; }
        public bool Published { get; set; }
        public string EmbedCode { get; set; }
        public bool IsApproved { get; set; }
        public int DisplayOrder { get; set; }
        public int PictureId { get; set; }
        public string PictureUrl { get; set; }
        public string ResimUrl { get; set; }

    }
}