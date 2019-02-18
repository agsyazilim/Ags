using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Enews
{
    /// <summary>
    /// Represents a news comment model
    /// </summary>
    public partial class ENewsCategoriesModel : BaseAgsEntityModel
    {
        #region Properties
        [AgsDisplayName("Kategori ismi")]
        public string Name { get; set; }
        #endregion
    }
}