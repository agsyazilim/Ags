using Ags.Services.Blogs;
using Ags.Services.Catalog;
using Ags.Services.News;
using Ags.Services.Seo;
using Ags.Services.Topics;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Controllers
{
    public partial class BackwardCompatibility2XController : BasePublicController
    {
        #region Fields

        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly INewsService _newsService;
        private readonly ITopicService _topicService;
        private readonly IUrlRecordService _urlRecordService;

        #endregion

        #region Ctor

        public BackwardCompatibility2XController(IBlogService blogService,
            ICategoryService categoryService,
            INewsService newsService,
            ITopicService topicService,
            IUrlRecordService urlRecordService)
        {
            this._blogService = blogService;
            this._categoryService = categoryService;
            this._newsService = newsService;
            this._topicService = topicService;
            this._urlRecordService = urlRecordService;
        }

        #endregion

        #region Methods



        //in versions 2.00-2.65 we had ID in category URLs
        public virtual IActionResult RedirectCategoryById(int categoryId)
        {
            var category = _categoryService.GetCategoryById(categoryId);
            if (category == null)
                return RedirectToRoutePermanent("HomePage");

            return RedirectToRoutePermanent("Category", new { SeName = _urlRecordService.GetSeName(category) });
        }



        //in versions 2.00-2.70 we had ID in news URLs
        public virtual IActionResult RedirectNewsItemById(int newsItemId)
        {
            var newsItem = _newsService.GetNewsById(newsItemId);
            if (newsItem == null)
                return RedirectToRoutePermanent("HomePage");

            return RedirectToRoutePermanent("NewsItem", new { SeName = _urlRecordService.GetSeName(newsItem) });
        }

        //in versions 2.00-2.70 we had ID in blog URLs
        public virtual IActionResult RedirectBlogPostById(int blogPostId)
        {
           var blogPost = _blogService.GetBlogPostById(blogPostId);
            if (blogPost == null)
                return RedirectToRoutePermanent("HomePage");

            return RedirectToRoutePermanent("BlogPost", new { SeName = _urlRecordService.GetSeName(blogPost) });
        }

        //in versions 2.00-3.20 we had SystemName in topic URLs
        public virtual IActionResult RedirectTopicBySystemName(string systemName)
        {
            var topic = _topicService.GetTopicBySystemName(systemName);
            if (topic == null)
                return RedirectToRoutePermanent("HomePage");

            return RedirectToRoutePermanent("Topic", new { SeName = _urlRecordService.GetSeName(topic) });
        }



        #endregion
    }
}