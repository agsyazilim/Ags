using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Polls
{
    /// <summary>
    /// Represents a poll answer model
    /// </summary>
    public partial class PollAnswerModel : BaseAgsEntityModel
    {
        #region Properties

        public int PollId { get; set; }

        [AgsDisplayName("Name :")]
        public string Name { get; set; }

        [AgsDisplayName("Cevap Sayısı :")]
        public int NumberOfVotes { get; set; }

        [AgsDisplayName("Sıarsı :")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}