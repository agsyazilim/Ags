using Ags.Data.Domain;
using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class BlogTagsViewComponent : AgsViewComponent
    {
        private readonly BlogSettings _blogSettings;
        private readonly ICustomerBlogFactory _customerBlogFactory;

        public BlogTagsViewComponent(BlogSettings blogSettings, IBlogModelFactory blogModelFactory, ICustomerBlogFactory customerBlogFactory)
        {
            this._blogSettings = blogSettings;
            _customerBlogFactory = customerBlogFactory;
        }

        public IViewComponentResult Invoke(int currentCategoryId, int currentProductId)
        {
            if (!_blogSettings.Enabled)
                return Content("");

            var model = _customerBlogFactory.PrepareBlogPostTagListModel();
            return View(model);
        }
    }
}
