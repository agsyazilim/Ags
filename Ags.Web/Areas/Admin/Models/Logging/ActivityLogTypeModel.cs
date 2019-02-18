using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Logging
{
    /// <summary>
    /// Represents an activity log type model
    /// </summary>
    public partial class ActivityLogTypeModel : BaseAgsEntityModel
    {
        #region Properties

        [AgsDisplayName("Adı :")]
        public string Name { get; set; }

        [AgsDisplayName("Aktif mi? :")]
        [UIHint("Boolean")]
        public bool Enabled { get; set; }

        #endregion
    }
}