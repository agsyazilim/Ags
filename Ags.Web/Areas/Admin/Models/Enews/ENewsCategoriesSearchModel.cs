using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Enews
{
    /// <summary>
    /// Represents a news comment search model
    /// </summary>
    public partial class ENewsCategoriesSearchModel : BaseSearchModel
    {

        #region Properties
        [AgsDisplayName("Aranan Kategori")]
        public string SearchText { get; set; }
        #endregion
    }
}