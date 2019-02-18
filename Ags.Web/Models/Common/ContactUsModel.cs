using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Common
{
    public partial class ContactUsModel : BaseAgsModel
    {
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Subject")]
        public string Subject { get; set; }
        public bool SubjectEnabled { get; set; }

        [DisplayName("Enquiry")]
        public string Enquiry { get; set; }

        [DisplayName("FullName")]
        public string FullName { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}