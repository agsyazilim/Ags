using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Email
{
    /// <summary>
    /// Represents an email account model
    /// </summary>
    public partial class EmailAccountModel : BaseAgsEntityModel
    {
        #region Properties

        [DataType(DataType.EmailAddress)]
        [Required]
        [AgsDisplayName("Email :")]
        public string Email { get; set; }
        [Required]
        [AgsDisplayName("Görünen Adı :")]
        public string DisplayName { get; set; }
        [Required]
        [AgsDisplayName("Host :")]
        public string Host { get; set; }
        [Required]
        [AgsDisplayName("Port :")]
        public int Port { get; set; }
        [Required]
        [AgsDisplayName("Kullanıcı Adı :")]
        public string Username { get; set; }
        [Required]
        [AgsDisplayName("Şifre :")]
        [DataType(DataType.Password)]
        [NoTrim]
        public string Password { get; set; }
        [AgsDisplayName("SSl Varmı :")]
        [UIHint("Boolean")]
        public bool EnableSsl { get; set; }
        [AgsDisplayName("UseDefaultCredentials :")]
        [UIHint("Boolean")]
        public bool UseDefaultCredentials { get; set; }
        [AgsDisplayName("Varsayılan :")]
        [UIHint("Boolean")]
        public bool IsDefaultEmailAccount { get; set; }
        [AgsDisplayName("Test Maili :")]
        public string SendTestEmailTo { get; set; }

        #endregion
    }
}