using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Topics
{
    /// <summary>
    /// Represents a topic search model
    /// </summary>
    public partial class TopicSearchModel : BaseSearchModel
    {

        [AgsDisplayName("SearchKeywords")]
        public string SearchKeywords { get; set; }


    }
}