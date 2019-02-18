using Ags.Web.Areas.Admin.Models.Templates;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the template model factory
    /// </summary>
    public partial interface ITemplateModelFactory
    {
        /// <summary>
        /// Prepare templates model
        /// </summary>
        /// <param name="model">Templates model</param>
        /// <returns>Templates model</returns>
        TemplatesModel PrepareTemplatesModel(TemplatesModel model);

        /// <summary>
        /// Prepare category template search model
        /// </summary>
        /// <param name="searchModel">Category template search model</param>
        /// <returns>Category template search model</returns>
        CategoryTemplateSearchModel PrepareCategoryTemplateSearchModel(CategoryTemplateSearchModel searchModel);

        /// <summary>
        /// Prepare paged category template list model
        /// </summary>
        /// <param name="searchModel">Category template search model</param>
        /// <returns>Category template list model</returns>
        CategoryTemplateListModel PrepareCategoryTemplateListModel(CategoryTemplateSearchModel searchModel);

       

        /// <summary>
        /// Prepare topic template search model
        /// </summary>
        /// <param name="searchModel">Topic template search model</param>
        /// <returns>Topic template search model</returns>
        TopicTemplateSearchModel PrepareTopicTemplateSearchModel(TopicTemplateSearchModel searchModel);

        /// <summary>
        /// Prepare paged topic template list model
        /// </summary>
        /// <param name="searchModel">Topic template search model</param>
        /// <returns>Topic template list model</returns>
        TopicTemplateListModel PrepareTopicTemplateListModel(TopicTemplateSearchModel searchModel);
    }
}