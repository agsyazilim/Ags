using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Catalog
{
    public class NewsPopulerModel:BaseAgsEntityModel
    {
        public string SeName { get; set; }
        public string PictureUrl { get; set; }
        public string Title { get; set; }
        public string Short { get; set; }
    }
}