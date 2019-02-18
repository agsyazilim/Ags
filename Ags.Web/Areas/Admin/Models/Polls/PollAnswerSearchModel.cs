using Ags.Web.Framework.Models;

namespace Ags.Web.Areas.Admin.Models.Polls
{
    /// <summary>
    /// Represents a poll answer search model
    /// </summary>
    public partial class PollAnswerSearchModel : BaseSearchModel
    {
        #region Properties

        public int PollId { get; set; }

        #endregion
    }
}