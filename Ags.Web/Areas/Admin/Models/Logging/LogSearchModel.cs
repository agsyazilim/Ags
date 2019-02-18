using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Logging
{
    /// <summary>
    /// Represents a log search model
    /// </summary>
    public partial class LogSearchModel : BaseSearchModel
    {
        #region Ctor

        public LogSearchModel()
        {
            AvailableLogLevels = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AgsDisplayName("Oluşturma")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnFrom { get; set; }

        [AgsDisplayName("Gönderilme")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

        [AgsDisplayName("Mesaj")]
        public string Message { get; set; }

        [AgsDisplayName("Log Seviyesi")]
        public int LogLevelId { get; set; }

        public IList<SelectListItem> AvailableLogLevels { get; set; }

        #endregion
    }
}