using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Areas.Admin.Models.Logging
{
    /// <summary>
    /// Represents an activity log container model
    /// </summary>
    public partial class ActivityLogContainerModel : BaseAgsModel
    {
        #region Ctor

        public ActivityLogContainerModel()
        {
            ListLogs = new ActivityLogSearchModel();
            ListTypes = new List<ActivityLogTypeModel>();
        }

        #endregion

        #region Properties

        public ActivityLogSearchModel ListLogs { get; set; }

        public IList<ActivityLogTypeModel> ListTypes { get; set; }

        #endregion
    }
}
