using System;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Polls
{
    /// <summary>
    /// Represents a poll model
    /// </summary>
    public partial class PollModel : BaseAgsEntityModel
    {
        #region Ctor

        public PollModel()
        {
            this.PollAnswerSearchModel = new PollAnswerSearchModel();
        }

        #endregion

        #region Properties
        [AgsDisplayName("Anket Adı :")]
        [Required]
        public string Name { get; set; }

        [AgsDisplayName("Sistem Adı :")]
        public string SystemKeyword { get; set; }

        [AgsDisplayName("Yayında :")]
        [UIHint("Boolean")]
        public bool Published { get; set; }

        [AgsDisplayName("Anasayfada Göster :")]
        [UIHint("Boolean")]
        public bool ShowOnHomePage { get; set; }

        [AgsDisplayName("Herkes Oylayabilir :")]
        [UIHint("Boolean")]
        public bool AllowGuestsToVote { get; set; }

        [AgsDisplayName("Sırası :")]
        public int DisplayOrder { get; set; }

        [AgsDisplayName("Başlama Tarihi :")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartDate { get; set; }

        [AgsDisplayName("Yayından Kaldırma Tarihi :")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDate { get; set; }



        public PollAnswerSearchModel PollAnswerSearchModel { get; set; }

        #endregion
    }
}