using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Newsletter
{
    public partial class NewsletterBoxModel : BaseAgsModel
    {
        [DataType(DataType.EmailAddress)]
        public string NewsletterEmail { get; set; }
        public bool AllowToUnsubscribe { get; set; }
    }
}