using System.Collections.Generic;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Media.Galery
{
    public class PhotoGallerySearchModel:BaseSearchModel
    {
        public PhotoGallerySearchModel()
        {
            AvailableGalleries = new List<SelectListItem>();
        }
        [AgsDisplayName("Adı")]
        public string SearchName { get; set; }

        public int SearchGalleryId { get; set; }

        public IList<SelectListItem> AvailableGalleries { get; set; }
    }
}