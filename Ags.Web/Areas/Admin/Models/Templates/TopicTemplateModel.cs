using Ags.Web.Framework.Models;
using Ags.Web.Framework.Mvc.ModelBinding;

namespace Ags.Web.Areas.Admin.Models.Templates
{
    /// <summary>
    /// Represents a topic template model
    /// </summary>
    public partial class TopicTemplateModel : BaseAgsEntityModel
    {
        #region Properties

        [AgsDisplayName("Adı :")]
        public string Name { get; set; }

        [AgsDisplayName("ViewPath :")]
        public string ViewPath { get; set; }

        [AgsDisplayName("Sırası :")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}