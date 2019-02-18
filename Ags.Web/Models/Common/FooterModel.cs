using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Common
{
    public partial class FooterModel : BaseAgsModel
    {
        public FooterModel()
        {
            Topics = new List<FooterTopicModel>();
        }

        public string StoreName { get; set; }


        public IList<FooterTopicModel> Topics { get; set; }

        public bool DisplaySitemapFooterItem { get; set; }
        public bool DisplayContactUsFooterItem { get; set; }
        public bool DisplayNewsFooterItem { get; set; }
        public bool DisplayBlogFooterItem { get; set; }

        #region Nested classes

        public class FooterTopicModel : BaseAgsEntityModel
        {
            public string Name { get; set; }
            public string SeName { get; set; }

            public bool IncludeInFooterColumn1 { get; set; }
            public bool IncludeInFooterColumn2 { get; set; }
            public bool IncludeInFooterColumn3 { get; set; }
        }
        
        #endregion
    }
}