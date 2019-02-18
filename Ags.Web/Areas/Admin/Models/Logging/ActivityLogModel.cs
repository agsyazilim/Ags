using System;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Logging
{
    /// <summary>
    /// Represents an activity log model
    /// </summary>
    public partial class ActivityLogModel : BaseAgsEntityModel
    {
        #region Properties

        [AgsDisplayName("Log  Tipi :")]
        public string ActivityLogTypeName { get; set; }

        [AgsDisplayName("Kullanıcı :")]
        public int CustomerId { get; set; }

        [AgsDisplayName("Kullanıcı Mail :")]
        public string CustomerEmail { get; set; }

        [AgsDisplayName("İçerik :")]
        public string Comment { get; set; }

        [AgsDisplayName("Oluşturma :")]
        public DateTime CreatedOn { get; set; }

        [AgsDisplayName("IpAddress :")]
        public string IpAddress { get; set; }

        #endregion
    }
}