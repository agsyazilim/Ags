using System.Collections.Generic;
using Ags.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Videos
{
    public class VideoKategoriSearchModel:BaseSearchModel
    {
        public VideoKategoriSearchModel()
        {
            AvailableVideoCategorys = new List<SelectListItem>();
        }

        public int SearchCategoriId { get; set; }
        public IList<SelectListItem> AvailableVideoCategorys { get; set; }
    }
}