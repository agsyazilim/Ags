using System.Collections.Generic;
using System.Linq;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Catalog
{
    public partial class TopMenuModel:BaseAgsModel
    {
        public TopMenuModel()
        {
            Categories = new List<CategorySimpleModel>();
            Topics = new List<TopicModel>();
        }

        public IList<CategorySimpleModel> Categories { get; set; }
        public IList<TopicModel> Topics { get; set; }
        public bool BlogEnabled { get; set; }
        public bool DisplayHomePageMenuItem { get; set; }
        public bool DisplayBlogMenuItem { get; set; }
        public bool DisplayContactUsMenuItem { get; set; }
        public bool HasOnlyCategories
        {
            get
            {
                return Categories.Any()
                       && !Topics.Any()
                       && !DisplayHomePageMenuItem
                       && !(DisplayBlogMenuItem && BlogEnabled)
                       && !DisplayContactUsMenuItem;
            }
        }

        #region Nested classes

        public class TopicModel
        {
            public string Name { get; set; }
            public string SeName { get; set; }
        }

        public class CategoryLineModel
        {
            public int Level { get; set; }
            public bool ResponsiveMobileMenu { get; set; }
            public CategorySimpleModel Category { get; set; }
        }

        #endregion
    }
}