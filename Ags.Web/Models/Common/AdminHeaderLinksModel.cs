using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Common
{
    public partial class AdminHeaderLinksModel : BaseAgsModel
    {

        public bool DisplayAdminLink { get; set; }
        public string EditPageUrl { get; set; }
    }
}