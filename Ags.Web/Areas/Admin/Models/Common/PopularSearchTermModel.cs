using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents a popular search term model
    /// </summary>
    public partial class PopularSearchTermModel : BaseAgsModel
    {
        #region Properties

        [AgsDisplayName("Arama Kelimeleri :")]
        public string Keyword { get; set; }

        [AgsDisplayName("Toplam :")]
        public int Count { get; set; }

        #endregion
    }
}
