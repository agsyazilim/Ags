using System;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Logging
{
    /// <summary>
    /// Represents a log model
    /// </summary>
    public partial class LogModel : BaseAgsEntityModel
    {
        #region Properties

        [AgsDisplayName("LogLevel :")]
        public string LogLevel { get; set; }

        [AgsDisplayName("Açıklama :")]
        public string ShortMessage { get; set; }

        [AgsDisplayName("FullMessage :")]
        public string FullMessage { get; set; }

        [AgsDisplayName("IPAddress :")]
        public string IpAddress { get; set; }

        [AgsDisplayName("Kullanıcı :")]
        public int? CustomerId { get; set; }

        [AgsDisplayName("Email :")]
        public string CustomerEmail { get; set; }

        [AgsDisplayName("PageURL :")]
        public string PageUrl { get; set; }

        [AgsDisplayName("ReferrerURL :")]
        public string ReferrerUrl { get; set; }

        [AgsDisplayName("Oluşturma .")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}