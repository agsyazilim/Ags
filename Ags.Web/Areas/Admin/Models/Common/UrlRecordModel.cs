using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents an URL record model
    /// </summary>
    public partial class UrlRecordModel : BaseAgsEntityModel
    {
        #region Properties

        [AgsDisplayName("Adı :")]
        public string Name { get; set; }
        [AgsDisplayName("EntityId :")]
        public int EntityId { get; set; }
        [AgsDisplayName("Tablo Adı :")]
        public string EntityName { get; set; }
        [AgsDisplayName("Yayında mı? :")]
        public bool IsActive { get; set; }
        [AgsDisplayName("Detay Url :")]
        public string DetailsUrl { get; set; }

        #endregion
    }
}