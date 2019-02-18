using System.Collections.Generic;
using Ags.Data.Core.Pages;
using Ags.Data.Domain.Catalog;

namespace Ags.Services.Catalog
{
    /// <summary>
    /// Category service interface
    /// </summary>
    public partial interface ICategoryService
    {
        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="category">Category</param>
        void DeleteCategory(Category category);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Categories</returns>
        IList<Category> GetAllCategories(int storeId = 0, bool showHidden = false, bool loadCacheableCopy = true);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        IPagedList<Category> GetAllCategories(string categoryName, int storeId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Gets all categories filtered by parent category identifier
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        IList<Category> GetAllCategoriesByParentCategoryId(int parentCategoryId, bool showHidden = false);

        /// <summary>
        /// Gets all categories displayed on the home page
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        IList<Category> GetAllCategoriesDisplayedOnHomePage(bool showHidden = false);

        /// <summary>
        /// Gets all categories displayed on the home page
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        List<Category> GetAllCategoriesInculdeMansetPage();
        /// <summary>
        /// Gets child category identifiers
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Category identifiers</returns>
        IList<int> GetChildCategoryIds(int parentCategoryId, int storeId = 0, bool showHidden = false);

        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>Category</returns>
        Category GetCategoryById(int categoryId);

        /// <summary>
        /// Inserts category
        /// </summary>
        /// <param name="category">Category</param>
        void InsertCategory(Category category);

        /// <summary>
        /// Updates the category
        /// </summary>
        /// <param name="category">Category</param>
        void UpdateCategory(Category category);
        /// <summary>
        /// Returns a list of names of not existing categories
        /// </summary>
        /// <param name="categoryIdsNames">The names and/or IDs of the categories to check</param>
        /// <returns>List of names and/or IDs not existing categories</returns>
        string[] GetNotExistingCategories(string[] categoryIdsNames);

        /// <summary>
        /// Get category IDs for products
        /// </summary>
        /// <param name="productIds">Products IDs</param>
        /// <returns>Category IDs for products</returns>
        IDictionary<int, int[]> GetNewsItemCategoryIds(int[] productIds);

        /// <summary>
        /// Gets categories by identifier
        /// </summary>
        /// <param name="categoryIds">Category identifiers</param>
        /// <returns>Categories</returns>
        List<Category> GetCategoriesByIds(int[] categoryIds);

        /// <summary>
        /// Sort categories for tree representation
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="parentId">Parent category identifier</param>
        /// <param name="ignoreCategoriesWithoutExistingParent">A value indicating whether categories without parent category in provided category list (source) should be ignored</param>
        /// <returns>Sorted categories</returns>
        IList<Category> SortCategoriesForTree(IList<Category> source, int parentId = 0,
            bool ignoreCategoriesWithoutExistingParent = false);
        /// <summary>
        /// Gets product category mapping collection
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product a category mapping collection</returns>
        IPagedList<CategoryNews> GetNewsItemCategoriesByCategoryId(int categoryId,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Returns a ProductCategory that has the specified values
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="newsId"></param>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>A ProductCategory that has the specified values; otherwise null</returns>
        CategoryNews FindNewsItemCategory(IList<CategoryNews> source, int newsId, int categoryId);

        /// <summary>
        /// Get formatted category breadcrumb
        /// Note: ACL and store mapping is ignored
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="allCategories">All categories</param>
        /// <param name="separator">Separator</param>
        /// <param name="languageId">Language identifier for localization</param>
        /// <returns>Formatted breadcrumb</returns>
        string GetFormattedBreadCrumb(Category category, IList<Category> allCategories = null,
            string separator = ">>", int languageId = 0);

        /// <summary>
        /// Get category breadcrumb
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="allCategories">All categories</param>
        /// <param name="showHidden">A value indicating whether to load hidden records</param>
        /// <returns>Category breadcrumb </returns>
        IList<Category> GetCategoryBreadCrumb(Category category, IList<Category> allCategories = null, bool showHidden = false);
        /// <summary>
        /// GetNewsItemCategoriesByNewsId
        /// </summary>
        /// <param name="newsItemId"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        IList<CategoryNews> GetNewsItemCategoriesByNewsId(int newsItemId, bool showHidden=false);
        /// <summary>
        /// GetNewsItemCategoriesByCategorysId
        /// </summary>
        /// <param name="newsItemId"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        Category GetNewsItemCategoriesByCategorysId(int newsItemId, bool showHidden = false);
        /// <summary>
        /// DeleteCategoryNews
        /// </summary>
        /// <param name="categoryNews"></param>
        void DeleteCategoryNews(CategoryNews categoryNews);
        /// <summary>
        /// DeleteCategoryNews
        /// </summary>
        /// <param name="categoryNewses"></param>
        void DeleteCategoryNews(List<CategoryNews> categoryNewses);
        /// <summary>
        /// InsertCategoryNews
        /// </summary>
        /// <param name="categoryNews"></param>
        void InsertCategoryNews(CategoryNews categoryNews);
        /// <summary>
        /// UpdateCatgoryNews
        /// </summary>
        /// <param name="categoryNews"></param>
        void UpdateCatgoryNews(CategoryNews categoryNews);
        /// <summary>
        /// GetSliderCategoryList
        /// </summary>
        /// <returns></returns>
        List<Category> GetSliderCategoryList();
        /// <summary>
        /// GetGallerCategoryList
        /// </summary>
        /// <returns></returns>
        List<Category> GetGallerCategoryList();
        /// <summary>
        /// GetVideosCategoryList
        /// </summary>
        /// <returns></returns>
        List<Category> GetVideosCategoryList();
    }
}