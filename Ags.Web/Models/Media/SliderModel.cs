using System;
using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Media
{
    public class SliderModel:BaseAgsEntityModel
    {
        public SliderModel()
        {
            SliderPictureModels = new List<SliderPictureModel>();
        }

        public string PictureUrl { get; set; }

        public int DisplayOrder { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string PictureTitle { get; set; }
        public string CategoryName { get; set; }
        public int PictureId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Published { get; set; }
        public string Name { get; set; }
        public string SectionName { get; set; }
        public List<SliderPictureModel> SliderPictureModels { get; set; }
    }
}