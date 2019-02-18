using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Tasks
{
    /// <summary>
    /// Represents a schedule task model
    /// </summary>
    public partial class ScheduleTaskModel : BaseAgsEntityModel
    {
        #region Properties

        [AgsDisplayName("Adı :")]
        public string Name { get; set; }

        [AgsDisplayName("Saniye :")]
        public int Seconds { get; set; }

        [AgsDisplayName("Aktif :")]
        [UIHint("Boolean")]
        public bool Enabled { get; set; }

        [AgsDisplayName("Hata da Dur :")]
        [UIHint("Boolean")]
        public bool StopOnError { get; set; }

        [AgsDisplayName("Başlama zamanı :")]
        public string LastStartUtc { get; set; }

        [AgsDisplayName("Bitiş Zamanı :")]
        public string LastEndUtc { get; set; }

        [AgsDisplayName("Tamamlandı :")]
        public string LastSuccessUtc { get; set; }

        #endregion
    }
}