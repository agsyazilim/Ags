using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Common
{
    public partial class SocialModel : BaseAgsModel
    {
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string YoutubeLink { get; set; }
        public string GooglePlusLink { get; set; }
        public string Instagram { get; set; }
        public bool NewsEnabled { get; set; }
    }
}