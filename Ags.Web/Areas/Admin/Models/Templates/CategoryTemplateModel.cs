using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Templates
{
    /// <summary>
    /// Represents a category template model
    /// </summary>
    public partial class CategoryTemplateModel : BaseAgsEntityModel
    {
        #region Properties

        [AgsDisplayName("Adı :")]
        public string Name { get; set; }

        [AgsDisplayName("ViewPath :")]
        public string ViewPath { get; set; }

        [AgsDisplayName("Sırası .")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}