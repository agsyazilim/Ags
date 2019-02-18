using Ags.Web.Framework.Models;

namespace Ags.Web.Areas.Admin.Models.Templates
{
    /// <summary>
    /// Represents a templates model
    /// </summary>
    public partial class TemplatesModel : BaseAgsModel
    {
        #region Ctor

        public TemplatesModel()
        {
            TemplatesCategory = new CategoryTemplateSearchModel();
            TemplatesTopic = new TopicTemplateSearchModel();
        }

        #endregion

        #region Properties

        public CategoryTemplateSearchModel TemplatesCategory { get; set; }

        public TopicTemplateSearchModel TemplatesTopic { get; set; }

        #endregion
    }
}
