using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Catalog
{
    public class MainPageDownModel: BaseAgsEntityModel
    {
        public MainPageDownModel()
        {
            MainCategoryModel = new MainDownCategoryModel();
        }
        public MainDownCategoryModel MainCategoryModel { get; set; }
    }

    public class MainDownCategoryModel : BaseAgsEntityModel
    {
        public MainDownCategoryModel()
        {

            NewsModels = new List<NewsPopulerModel>();
        }
        public string Name { get; set; }
        public string SeName { get; set; }
        public IList<NewsPopulerModel> NewsModels { get; set; }

    }
}