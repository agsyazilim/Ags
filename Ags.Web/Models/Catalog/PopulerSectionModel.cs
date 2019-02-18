using System.Collections.Generic;
using Ags.Web.Framework.Models;
using Ags.Web.Models.News;

namespace Ags.Web.Models.Catalog
{
    public class PopulerSectionModel: BaseAgsEntityModel
    {
        public PopulerSectionModel()
        {
            NewsModels = new List<NewsPopulerModel>();
        }
       public IList<NewsPopulerModel> NewsModels { get; set; }
    }
}