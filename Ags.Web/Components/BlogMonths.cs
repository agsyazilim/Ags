using Ags.Data.Domain;
using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class BlogMonthsViewComponent : AgsViewComponent
    {
        private readonly BlogSettings _blogSettings;
        private readonly ICustomerBlogFactory _customerBlogFactory;

        public BlogMonthsViewComponent(BlogSettings blogSettings, ICustomerBlogFactory customerBlogFactory)
        {
            this._blogSettings = blogSettings;
            _customerBlogFactory = customerBlogFactory;
        }

        public IViewComponentResult Invoke(int currentCategoryId, int currentProductId)
        {
            if (!_blogSettings.Enabled)
                return Content("");

            var model = _customerBlogFactory.PrepareBlogPostYearModel();
            return View(model);
        }
    }
}
