using System.Collections.Generic;
using Ags.Data.Domain.Catalog;
using Ags.Web.Models.Catalog;

namespace Ags.Web.Factories
{
    public partial interface ICatalogModelFactory
    {
        #region Common




        /// <summary>
        /// Prepare page size options
        /// </summary>
        /// <param name="pagingFilteringModel">Catalog paging filtering model</param>
        /// <param name="command">Catalog paging filtering command</param>
        /// <param name="pageSizeOptions">Page size options</param>
        /// <param name="fixedPageSize">Fixed page size</param>
        void PreparePageSizeOptions(CatalogPagingFilteringModel pagingFilteringModel, CatalogPagingFilteringModel command,
           string pageSizeOptions, int fixedPageSize);

        #endregion

        #region Categories

        /// <summary>
        /// Prepare category model
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="command">Catalog paging filtering command</param>
        /// <returns>Category model</returns>
        CategoriModel PrepareCategoryModel(Category category, CatalogPagingFilteringModel command);
        CategoriModel PrepareCategoryModel(CategoriModel model, Category category);
        CategoriModel PrepareCategoryModel(CategoriModel model, List<Category> categorys);
        PopulerSectionModel PrepareCategoryPopulerSectionModel(List<Category> categorys);


        /// <summary>
        /// Prepare category template view path
        /// </summary>
        /// <param name="templateId">Template identifier</param>
        /// <returns>Category template view path</returns>
        string PrepareCategoryTemplateViewPath(int templateId);


        #endregion


    }
}
