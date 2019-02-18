using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Catalog
{
    public class CategorySimpleModel : BaseAgsEntityModel
    {

        public string Name { get; set; }

        public string SeName { get; set; }

        public int ParentCategoryId { get; set; }

        public bool IncludeInTopMenu { get; set; }
        


    }
}