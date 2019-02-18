using System.Net;
using Ags.Data.Core;
using Ags.Data.Core.Http;
using Ags.Services.Tasks;

namespace Ags.Services.Common
{
    /// <summary>
    /// Represents a task for keeping the site alive
    /// </summary>
    public partial class KeepAliveTask : IScheduleTask
    {
        #region Fields

        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public KeepAliveTask(IWebHelper webHelper)
        {
            this._webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute()
        {
            var keepAliveUrl = $"{_webHelper.GetStoreLocation()}{AgsHttpDefaults.KeepAlivePath}";
            using (var wc = new WebClient())
            {
                wc.DownloadString(keepAliveUrl);
            }
        }

        #endregion
    }
}