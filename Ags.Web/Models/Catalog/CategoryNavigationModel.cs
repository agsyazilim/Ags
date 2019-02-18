using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Catalog
{
    public partial class CategoryNavigationModel :BaseAgsModel
    {
        public CategoryNavigationModel()
        {
            Categories = new List<CategorySimpleModel>();
        }

        public int CurrentCategoryId { get; set; }
        public List<CategorySimpleModel> Categories { get; set; }

        #region Nested classes

        public class CategoryLineModel 
        {
            public int CurrentCategoryId { get; set; }
            public CategorySimpleModel Category { get; set; }
        }

        #endregion
    }
}