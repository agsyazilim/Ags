using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Caching;
using Ags.Data.Domain.Catalog;
using Ags.Services.Catalog;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Helpers
{
    /// <summary>
    /// Select list helper
    /// </summary>
    public static class SelectListHelper
    {
        /// <summary>
        /// Get category list
        /// </summary>
        /// <param name="categoryService">Category service</param>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Category list</returns>
        public static List<SelectListItem> GetCategoryList(ICategoryService categoryService, ICacheManager cacheManager, bool showHidden = false)
        {
            if (categoryService == null)
                throw new ArgumentNullException(nameof(categoryService));

            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));

            //var cacheKey = string.Format(ModelCacheEventConsumer.CATEGORIES_LIST_KEY, showHidden);
            //var listItems = cacheManager.Get(cacheKey, () =>
            //{

            //});
            IList<Category> categories = categoryService.GetAllCategories(showHidden: showHidden, loadCacheableCopy: false);
            IEnumerable<SelectListItem> listItems = categories.Select(c => new SelectListItem
            {
                Text = categoryService.GetFormattedBreadCrumb(c, categories),
                Value = c.Id.ToString()
            });
            List<SelectListItem> result = new List<SelectListItem>();
            //clone the list to ensure that "selected" property is not set
            foreach (SelectListItem item in listItems)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return result;
        }


    }
}