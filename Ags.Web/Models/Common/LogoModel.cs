using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Common
{
    public partial class LogoModel : BaseAgsModel
    {
        public string StoreName { get; set; }

        public string LogoPath { get; set; }
    }
}