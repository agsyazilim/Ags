using System;
using System.Collections.Generic;
using System.Linq;
using AdminLTE.Core;
using Ags.Data.Common;
using Ags.Data.Core.Caching;
using Ags.Data.Core.Pages;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Catalog;
using Ags.Data.Domain.News;
using Ags.Services.Events;

namespace Ags.Services.Catalog
{
    /// <summary>
    /// Category service
    /// </summary>
    public partial class CategoryService : ICategoryService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<NewsItem> _newsRepository;
        private readonly IRepository<CategoryNews> _categoryNewsRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IStoreContext _storeContext;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public CategoryService(
            ICacheManager cacheManager,
            IRepository<Category> categoryRepository,
            IRepository<NewsItem> newsRepository,
            IRepository<CategoryNews> categoryNewsCategoryRepository,
            IStaticCacheManager staticCacheManager,
            IStoreContext storeContext,
            IRepository<CategoryNews> categoryNewsRepository, IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._newsRepository = newsRepository;
            this._categoryRepository = categoryRepository;
            this._staticCacheManager = staticCacheManager;
            this._storeContext = storeContext;
            this._categoryNewsRepository = categoryNewsRepository;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="category">Category</param>
        public virtual void DeleteCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            if (category is IEntityForCaching)
            {
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");
            }

            category.Deleted = true;
            UpdateCategory(category);
             _eventPublisher.EntityDeleted(category);
            //reset a "Parent category" property of all child subcategories
            IList<Category> subcategories = GetAllCategoriesByParentCategoryId(category.Id, true);
            foreach (Category subcategory in subcategories)
            {
                subcategory.ParentCategoryId = 0;
                UpdateCategory(subcategory);
            }

        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Categories</returns>
        public virtual IList<Category> GetAllCategories(int storeId = 0, bool showHidden = false, bool loadCacheableCopy = true)
        {
            Func<IList<Category>> loadCategoriesFunc = () =>
            {
                return GetAllCategories("", storeId: storeId, showHidden: showHidden);
            };

            IList<Category> categories;
            if (loadCacheableCopy)
            {
                //cacheable copy
                string key = string.Format(AgsCatalogDefaults.CategoriesAllCacheKey,storeId, showHidden);
                categories = _staticCacheManager.Get(key, () =>
                {
                    var result = new List<Category>();
                    foreach (Category category in loadCategoriesFunc())
                    {
                        result.Add(new CategoryForCaching(category));
                    }

                    return result;
                });
            }
            else
            {
                categories = loadCategoriesFunc();
            }

            return categories;
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="categoryName">Category name</param>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public virtual IPagedList<Category> GetAllCategories(string categoryName, int storeId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {


            //don't use a stored procedure. Use LINQ
           var query = _categoryRepository.Table;
            if (!showHidden)
            {
                query = query.Where(c => c.Published);
            }

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                query = query.Where(c => c.Name.Contains(categoryName));
            }
            query = query.Where(c => !c.Deleted);
            query = query.OrderBy(c => c.ParentCategoryId).ThenBy(c => c.DisplayOrder).ThenBy(c => c.Id);
            var unsortedCategories = query.ToList();

            //sort categories
            var sortedCategories = this.SortCategoriesForTree(unsortedCategories);
            //paging
            return new PagedList<Category>(sortedCategories, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets all categories filtered by parent category identifier
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public virtual IList<Category> GetAllCategoriesByParentCategoryId(int parentCategoryId,
            bool showHidden = false)
        {
            IQueryable<Category> query = _categoryRepository.Table;
            if (!showHidden)
            {
                query = query.Where(c => c.Published);
            }

            query = query.Where(c => c.ParentCategoryId == parentCategoryId);
            query = query.Where(c => !c.Deleted);
            query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id);

            if (!showHidden)
            {
                query = query.Distinct().OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id);
            }

            List<Category> categories = query.ToList();
            return categories;
        }

        /// <summary>
        /// Gets all categories displayed on the home page
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public virtual IList<Category> GetAllCategoriesDisplayedOnHomePage(bool showHidden = false)
        {
            var query = from c in _categoryRepository.Table
                        orderby c.DisplayOrder, c.Id
                        where c.Published &&
                        !c.Deleted &&
                        c.ShowOnHomePage
                        select c;

            List<Category> categories = query.ToList();
            if (!showHidden)
            {
                categories = categories
                    .ToList();
            }

            return categories;
        }

        public List<Category> GetAllCategoriesInculdeMansetPage()
        {
            var query = from c in _categoryRepository.Table
                orderby c.DisplayOrder, c.Id
                where c.Published &&
                      !c.Deleted &&
                      c.IncludeInManset
                select c;

            List<Category> categories = query.ToList();
           return categories;
        }

        /// <summary>
        /// Gets child category identifiers
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Category identifiers</returns>
        public virtual IList<int> GetChildCategoryIds(int parentCategoryId, int storeId = 0, bool showHidden = false)
        {
            //little hack for performance optimization
            //there's no need to invoke "GetAllCategoriesByParentCategoryId" multiple times (extra SQL commands) to load childs
            //so we load all categories at once (we know they are cached) and process them server-side
            List<int> categoriesIds = new List<int>();
            IEnumerable<Category> categories = GetAllCategories(storeId: storeId, showHidden: showHidden)
                .Where(c => c.ParentCategoryId == parentCategoryId);
            foreach (Category category in categories)
            {
                categoriesIds.Add(category.Id);
                categoriesIds.AddRange(GetChildCategoryIds(category.Id, storeId, showHidden));
            }

            return categoriesIds;
        }

        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>Category</returns>
        public virtual Category GetCategoryById(int categoryId)
        {
            if (categoryId == 0)
            {
                return null;
            }


            return _categoryRepository.GetById(categoryId);
        }

        /// <summary>
        /// Inserts category
        /// </summary>
        /// <param name="category">Category</param>
        public virtual void InsertCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            if (category is IEntityForCaching)
            {
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");
            }

            _categoryRepository.Insert(category);

            //cache
            _cacheManager.RemoveByPattern(AgsCatalogDefaults.CategoriesPatternCacheKey);
            _staticCacheManager.RemoveByPattern(AgsCatalogDefaults.CategoriesPatternCacheKey);
            _cacheManager.RemoveByPattern(AgsCatalogDefaults.NewsItemCategoriesPatternCacheKey);
            _eventPublisher.EntityInserted(category);
        }

        /// <summary>
        /// Updates the category
        /// </summary>
        /// <param name="category">Category</param>
        public virtual void UpdateCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            if (category is IEntityForCaching)
            {
                throw new ArgumentException("Cacheable entities are not supported by Entity Framework");
            }

            //validate category hierarchy
            Category parentCategory = GetCategoryById(category.ParentCategoryId);
            while (parentCategory != null)
            {
                if (category.Id == parentCategory.Id)
                {
                    category.ParentCategoryId = 0;
                    break;
                }
                parentCategory = GetCategoryById(parentCategory.ParentCategoryId);
            }


            _eventPublisher.EntityUpdated(category);

            //cache
            _cacheManager.RemoveByPattern(AgsCatalogDefaults.CategoriesPatternCacheKey);
            _staticCacheManager.RemoveByPattern(AgsCatalogDefaults.CategoriesPatternCacheKey);
            _cacheManager.RemoveByPattern(AgsCatalogDefaults.NewsItemCategoriesPatternCacheKey);
            _categoryRepository.Update(category);
        }

        /// <summary>
        /// Deletes a product category mapping
        /// </summary>
        /// <param name="productCategory">Product category</param>
        public virtual void DeleteProductCategory(CategoryNews productCategory)
        {
            if (productCategory == null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            _categoryNewsRepository.Delete(productCategory);


            //cache
            _cacheManager.RemoveByPattern(AgsCatalogDefaults.NewsItemCategoriesPatternCacheKey);

            _eventPublisher.EntityDeleted(productCategory);
        }

        /// <summary>
        /// Gets product category mapping collection
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product a category mapping collection</returns>
        public virtual IPagedList<CategoryNews> GetNewsItemCategoriesByCategoryId(int categoryId,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            if (categoryId == 0)
            {
                return new PagedList<CategoryNews>(new List<CategoryNews>(), pageIndex, pageSize);
            }

            IQueryable<CategoryNews> query = from pc in _categoryNewsRepository.Table
                        join p in _newsRepository.Table on pc.NewsId equals p.Id
                        where pc.CategoryId == categoryId && (showHidden || p.Published)
                        orderby pc.DisplayOrder, pc.Id
                        select pc;

            if (!showHidden)
            {
                query = query.Distinct().OrderBy(pc => pc.DisplayOrder).ThenBy(pc => pc.Id);
            }

            PagedList<CategoryNews> productCategories = new PagedList<CategoryNews>(query, pageIndex, pageSize);
            return productCategories;
        }

        public CategoryNews FindNewsItemCategory(IList<CategoryNews> source, int newsId, int categoryId)
        {
            IQueryable<CategoryNews> query = (from cn in _categoryNewsRepository.Table
                join c in source on cn.CategoryId equals c.Id
                where cn.NewsId == newsId & cn.CategoryId==categoryId
                select cn);

            return query.FirstOrDefault();

        }

        /// <summary>
        /// Gets a product category mapping collection
        /// </summary>
        /// <param name="newsId"></param>
        /// <param name="showHidden"> A value indicating whether to show hidden records</param>
        /// <returns> Product category mapping collection</returns>
        public virtual IList<CategoryNews> GetNewsItemCategoriesByNewsId(int newsId, bool showHidden = false)
        {
            return GetNewsItemCategoriesByNewsId(newsId, _storeContext.CurrentStore.Id, showHidden);
        }

        public Category GetNewsItemCategoriesByCategorysId(int newsItemId, bool showHidden = false)
        {
            if (newsItemId == 0)
            {
                return new Category();
            }

            IQueryable<Category> query = from pc in _categoryNewsRepository.Table
                        join c in _categoryRepository.Table on pc.CategoryId equals c.Id
                        where pc.NewsId == newsItemId &&
                              !c.Deleted &&
                              (showHidden || c.Published)
                        orderby pc.DisplayOrder, pc.Id
                        select c;

            return query.FirstOrDefault();
        }

        public void DeleteCategoryNews(CategoryNews categoryNews)
        {
            if(categoryNews==null)
                throw new ArgumentNullException(nameof(categoryNews));
            _categoryNewsRepository.Delete(categoryNews);

        }

        public void DeleteCategoryNews(List<CategoryNews> categoryNewses)
        {
            if(categoryNewses ==null)
                throw new ArgumentNullException(nameof(categoryNewses));
            foreach (var categoryNewse in categoryNewses)
            {
                DeleteCategoryNews(categoryNewses);
            }
        }

        public void InsertCategoryNews(CategoryNews categoryNews)
        {
            if(categoryNews==null)
                throw new ArgumentNullException(nameof(categoryNews));
            _categoryNewsRepository.Insert(categoryNews);
        }

        public void UpdateCatgoryNews(CategoryNews categoryNews)
        {
          if(categoryNews==null)
              throw new ArgumentNullException(nameof(categoryNews));
          _categoryNewsRepository.Update(categoryNews);
        }

        public List<Category> GetSliderCategoryList()
        {
            var sorgu = _categoryRepository.Table;
            sorgu = sorgu.Where(x => !x.Deleted & x.Published & x.ShowOnHomePage & x.SliderId > 0);
            var result = sorgu.ToList();
            return result;
        }

        public List<Category> GetGallerCategoryList()
        {
            var sorgu = _categoryRepository.Table;
            sorgu = sorgu.Where(x => !x.Deleted & x.Published & x.ShowOnHomePage & x.PhotoGalleryId > 0);
            var result = sorgu.ToList();
            return result;
        }

        public List<Category> GetVideosCategoryList()
        {
            var sorgu = _categoryRepository.Table;
            sorgu = sorgu.Where(x => !x.Deleted & x.Published & x.ShowOnHomePage & x.VideoGalleryId > 0);
            var result = sorgu.ToList();
            return result;
        }

        /// <summary>
        /// Gets a product category mapping collection
        /// </summary>
        /// <param name="newsId">Product identifier</param>
        /// <param name="storeId">Store identifier (used in multi-store environment). "showHidden" parameter should also be "true"</param>
        /// <param name="showHidden"> A value indicating whether to show hidden records</param>
        /// <returns> Product category mapping collection</returns>
        public virtual IList<CategoryNews> GetNewsItemCategoriesByNewsId(int newsId, int storeId, bool showHidden = false)
        {
            if (newsId == 0)
            {
                return new List<CategoryNews>();
            }

            IQueryable<CategoryNews> query = from pc in _categoryNewsRepository.Table
                        join c in _categoryRepository.Table on pc.CategoryId equals c.Id
                        where pc.NewsId == newsId &&
                              !c.Deleted &&
                              (showHidden || c.Published)
                        orderby pc.DisplayOrder, pc.Id
                        select pc;

            List<CategoryNews> result = query.ToList();
            return result;
        }

        /// <summary>
        /// Gets a product category mapping
        /// </summary>
        /// <param name="productCategoryId">Product category mapping identifier</param>
        /// <returns>Product category mapping</returns>
        public virtual CategoryNews GetProductCategoryById(int productCategoryId)
        {
            if (productCategoryId == 0)
            {
                return null;
            }

            return _categoryNewsRepository.GetById(productCategoryId);
        }

        /// <summary>
        /// Inserts a product category mapping
        /// </summary>
        /// <param name="productCategory">>Product category mapping</param>
        public virtual void InsertProductCategory(CategoryNews productCategory)
        {
            if (productCategory == null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            _categoryNewsRepository.Insert(productCategory);

            //cache
            _cacheManager.RemoveByPattern(AgsCatalogDefaults.NewsItemCategoriesAllByNewsIdCacheKey);
        }

        /// <summary>
        /// Updates the product category mapping
        /// </summary>
        /// <param name="productCategory">>Product category mapping</param>
        public virtual void UpdateProductCategory(CategoryNews productCategory)
        {
            if (productCategory == null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            _categoryNewsRepository.Update(productCategory);

            //cache
            _cacheManager.RemoveByPattern(AgsCatalogDefaults.NewsItemCategoriesPatternCacheKey);

        }

        /// <summary>
        /// Returns a list of names of not existing categories
        /// </summary>
        /// <param name="categoryIdsNames">The names and/or IDs of the categories to check</param>
        /// <returns>List of names and/or IDs not existing categories</returns>
        public virtual string[] GetNotExistingCategories(string[] categoryIdsNames)
        {
            if (categoryIdsNames == null)
            {
                throw new ArgumentNullException(nameof(categoryIdsNames));
            }

            IQueryable<Category> query = _categoryRepository.Table;
            string[] queryFilter = categoryIdsNames.Distinct().ToArray();
            //filtering by name
            List<string> filter = query.Select(c => c.Name).Where(c => queryFilter.Contains(c)).ToList();
            queryFilter = queryFilter.Except(filter).ToArray();

            //if some names not found
            if (queryFilter.Any())
            {
                //filtering by IDs
                filter = query.Select(c => c.Id.ToString()).Where(c => queryFilter.Contains(c)).ToList();
                queryFilter = queryFilter.Except(filter).ToArray();
            }

            return queryFilter.ToArray();
        }



        /// <summary>
        /// Get category IDs for products
        /// </summary>
        /// <param name="productIds">Products IDs</param>
        /// <returns>Category IDs for products</returns>
        public virtual IDictionary<int, int[]> GetNewsItemCategoryIds(int[] productIds)
        {
            IQueryable<CategoryNews> query = _categoryNewsRepository.Table;

            return query.Where(p => productIds.Contains(p.NewsId))
                .Select(p => new { p.NewsId, p.CategoryId }).ToList()
                .GroupBy(a => a.NewsId)
                .ToDictionary(items => items.Key, items => items.Select(a => a.CategoryId).ToArray());
        }

        /// <summary>
        /// Gets categories by identifier
        /// </summary>
        /// <param name="categoryIds">Category identifiers</param>
        /// <returns>Categories</returns>
        public virtual List<Category> GetCategoriesByIds(int[] categoryIds)
        {
            if (categoryIds == null || categoryIds.Length == 0)
            {
                return new List<Category>();
            }

            IQueryable<Category> query = from p in _categoryRepository.Table
                        where categoryIds.Contains(p.Id) && !p.Deleted
                        select p;

            return query.ToList();
        }

        /// <summary>
        /// Sort categories for tree representation
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="parentId">Parent category identifier</param>
        /// <param name="ignoreCategoriesWithoutExistingParent">A value indicating whether categories without parent category in provided category list (source) should be ignored</param>
        /// <returns>Sorted categories</returns>
        public virtual IList<Category> SortCategoriesForTree(IList<Category> source, int parentId = 0,
            bool ignoreCategoriesWithoutExistingParent = false)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            List<Category> result = new List<Category>();

            foreach (Category cat in source.Where(c => c.ParentCategoryId == parentId).ToList())
            {
                result.Add(cat);
                result.AddRange(SortCategoriesForTree(source, cat.Id, true));
            }
            if (!ignoreCategoriesWithoutExistingParent && result.Count != source.Count)
            {
                //find categories without parent in provided category source and insert them into result
                foreach (Category cat in source)
                {
                    if (result.FirstOrDefault(x => x.Id == cat.Id) == null)
                    {
                        result.Add(cat);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Returns a ProductCategory that has the specified values
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="productId">Product identifier</param>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>A ProductCategory that has the specified values; otherwise null</returns>
        public virtual CategoryNews FindNewsCategory(IList<CategoryNews> source, int productId, int categoryId)
        {
            foreach (CategoryNews productCategory in source)
            {
                if (productCategory.NewsId == productId && productCategory.CategoryId == categoryId)
                {
                    return productCategory;
                }
            }

            return null;
        }

        /// <summary>
        /// Get formatted category breadcrumb
        /// Note: ACL and store mapping is ignored
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="allCategories">All categories</param>
        /// <param name="separator">Separator</param>
        /// <param name="languageId">Language identifier for localization</param>
        /// <returns>Formatted breadcrumb</returns>
        public virtual string GetFormattedBreadCrumb(Category category, IList<Category> allCategories = null,
            string separator = ">>", int languageId = 0)
        {
            string result = string.Empty;

            IList<Category> breadcrumb = this.GetCategoryBreadCrumb(category, allCategories, true);
            for (int i = 0; i <= breadcrumb.Count - 1; i++)
            {
                string categoryName = breadcrumb[i].Name;
                result = string.IsNullOrEmpty(result) ? categoryName : $"{result} {separator} {categoryName}";
            }

            return result;
        }

        /// <summary>
        /// Get category breadcrumb
        /// </summary>
        /// <param name="category">Category</param>
        /// <param name="allCategories">All categories</param>
        /// <param name="showHidden">A value indicating whether to load hidden records</param>
        /// <returns>Category breadcrumb </returns>
        public virtual IList<Category> GetCategoryBreadCrumb(Category category, IList<Category> allCategories = null, bool showHidden = false)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            List<Category> result = new List<Category>();

            //used to prevent circular references
            List<int> alreadyProcessedCategoryIds = new List<int>();

            while (category != null && //not null
                !category.Deleted && //not deleted
                (showHidden || category.Published) && //published
                !alreadyProcessedCategoryIds.Contains(category.Id)) //prevent circular references
            {
                result.Add(category);
                alreadyProcessedCategoryIds.Add(category.Id);
                category = allCategories != null ? allCategories.FirstOrDefault(c => c.Id == c.ParentCategoryId)
                    : this.GetCategoryById(category.ParentCategoryId);
            }
            result.Reverse();
            return result;
        }

        #endregion
    }
}