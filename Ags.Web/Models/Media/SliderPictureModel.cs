using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Media
{
    public class SliderPictureModel:BaseAgsEntityModel
    {
        public SliderPictureModel()
        {
            PictureModel = new PictureModel();
            
        }
        public int PictureId { get; set; }
        public int SliderId { get; set; }
        public string SliderTitle { get; set; }
        public string Url { get; set; }
        public string PictureTitle { get; set; }
        public PictureModel PictureModel { get; set; }
    }
}