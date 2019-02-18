using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Catalog
{
    public class MainPageDownCenterModel : BaseAgsEntityModel
    {
        public MainPageDownCenterModel()
        {
            MainCategoryModel = new MainDownCenterCategoryModel();
        }
        public MainDownCenterCategoryModel MainCategoryModel { get; set; }
    }

    public class MainDownCenterCategoryModel : BaseAgsEntityModel
    {
        public MainDownCenterCategoryModel()
        {

            LargeNewsModels = new List<NewsPopulerModel>();
        }
        public string Name { get; set; }
        public string SeName { get; set; }
        public IList<NewsPopulerModel> LargeNewsModels { get; set; }

    }
}