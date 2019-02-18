using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Catalog
{
    public class MainPageModel: BaseAgsEntityModel
    {
        public MainPageModel()
        {
            MainCategoryModel = new List<MainCategoryModel>();
        }

        public List<MainCategoryModel> MainCategoryModel { get; set; }
    }

    public class MainCategoryModel:BaseAgsEntityModel
    {
        public MainCategoryModel()
        {
            NewsBigModels = new List<NewsPopulerModel>();
            NewsModels = new List<NewsPopulerModel>();
        }
        public string Name { get; set; }
        public string SeName { get; set; }
        public IList<NewsPopulerModel> NewsModels { get; set; }
        public IList<NewsPopulerModel> NewsBigModels { get; set; }
    }
}