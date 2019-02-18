using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Models.Logging
{
    /// <summary>
    /// Represents an activity log search model
    /// </summary>
    public partial class ActivityLogSearchModel : BaseSearchModel
    {
        #region Ctor

        public ActivityLogSearchModel()
        {
            ActivityLogType = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AgsDisplayName("Oluþturma")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnFrom { get; set; }

        [AgsDisplayName("Gönderilme")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

        [AgsDisplayName("Log Tipi")]
        public int ActivityLogTypeId { get; set; }

        [AgsDisplayName("ActivityLogType")]
        public IList<SelectListItem> ActivityLogType { get; set; }

        [AgsDisplayName("IpAddress")]
        public string IpAddress { get; set; }

        #endregion
    }
}