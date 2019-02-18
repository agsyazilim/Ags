using System;
using System.Text.RegularExpressions;
using Ags.Data.Core;
using Ags.Services.Blogs;
using Ags.Services.Catalog;
using Ags.Services.Customers;
using Ags.Services.News;
using Ags.Services.Seo;
using Ags.Services.Topics;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Controllers
{
    public partial class BackwardCompatibility1XController : BasePublicController
    {
        #region Fields

        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly ICustomerService _customerService;
        private readonly INewsService _newsService;
        private readonly ITopicService _topicService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public BackwardCompatibility1XController(IBlogService blogService,
            ICategoryService categoryService,
            ICustomerService customerService,
            INewsService newsService,
            ITopicService topicService,
            IUrlRecordService urlRecordService,
            IWebHelper webHelper)
        {
            this._blogService = blogService;
            this._categoryService = categoryService;
            this._customerService = customerService;
            this._newsService = newsService;
            this._topicService = topicService;
            this._urlRecordService = urlRecordService;
            this._webHelper = webHelper;
        }

        #endregion

        #region Methods

        public virtual IActionResult GeneralRedirect()
        {
            // use Request.RawUrl, for instance to parse out what was invoked
            // this regex will extract anything between a "/" and a ".aspx"
            Regex regex = new Regex(@"(?<=/).+(?=\.aspx)", RegexOptions.Compiled);
            string rawUrl = _webHelper.GetRawUrl(this.HttpContext.Request);
            string aspxfileName = regex.Match(rawUrl).Value.ToLowerInvariant();

            switch (aspxfileName)
            {
                //URL without rewriting

                case "category":
                    {
                        return RedirectCategory(_webHelper.QueryString<string>("categoryid"), false);
                    }
                case "news":
                    {
                        return RedirectNewsItem(_webHelper.QueryString<string>("newsid"), false);
                    }
                case "blog":
                    {
                        return RedirectBlogPost(_webHelper.QueryString<string>("blogpostid"), false);
                    }
                case "topic":
                    {
                        return RedirectTopic(_webHelper.QueryString<string>("topicid"), false);
                    }
               case "contactus":
                    {
                        return RedirectToRoutePermanent("ContactUs");
                    }
                case "passwordrecovery":
                    {
                        return RedirectToRoutePermanent("PasswordRecovery");
                    }
                case "login":
                    {
                        return RedirectToRoutePermanent("Login");
                    }
                case "register":
                    {
                        return RedirectToRoutePermanent("Register");
                    }
                case "newsarchive":
                    {
                        return RedirectToRoutePermanent("NewsArchive");
                    }
                default:
                    break;
            }

            //no permanent redirect in this case
            return RedirectToRoute("HomePage");
        }


        public virtual IActionResult RedirectCategory(string id, bool idIncludesSename = true)
        {
            //we can't use dash in MVC
            int categoryid = idIncludesSename ? Convert.ToInt32(id.Split(new[] { '-' })[0]) : Convert.ToInt32(id);
            var category = _categoryService.GetCategoryById(categoryid);
            if (category == null)
                return RedirectToRoutePermanent("HomePage");

            return RedirectToRoutePermanent("Category", new { SeName = _urlRecordService.GetSeName(category) });
        }


        public virtual IActionResult RedirectNewsItem(string id, bool idIncludesSename = true)
        {
            //we can't use dash in MVC
            int newsId = idIncludesSename ? Convert.ToInt32(id.Split(new[] { '-' })[0]) : Convert.ToInt32(id);
            var newsItem = _newsService.GetNewsById(newsId);
            if (newsItem == null)
                return RedirectToRoutePermanent("HomePage");

            return RedirectToRoutePermanent("NewsItem", new { newsItemId = newsItem.Id, SeName = _urlRecordService.GetSeName(newsItem) });
        }

        public virtual IActionResult RedirectBlogPost(string id, bool idIncludesSename = true)
        {
            //we can't use dash in MVC
            int blogPostId = idIncludesSename ? Convert.ToInt32(id.Split(new[] { '-' })[0]) : Convert.ToInt32(id);
            var blogPost = _blogService.GetBlogPostById(blogPostId);
            if (blogPost == null)
                return RedirectToRoutePermanent("HomePage");

            return RedirectToRoutePermanent("BlogPost", new { blogPostId = blogPost.Id, SeName = _urlRecordService.GetSeName(blogPost) });
        }

        public virtual IActionResult RedirectTopic(string id, bool idIncludesSename = true)
        {
            //we can't use dash in MVC
            int topicid = idIncludesSename ? Convert.ToInt32(id.Split(new[] { '-' })[0]) : Convert.ToInt32(id);
           var topic = _topicService.GetTopicById(topicid);
            if (topic == null)
                return RedirectToRoutePermanent("HomePage");

            return RedirectToRoutePermanent("Topic", new { SeName = _urlRecordService.GetSeName(topic) });
        }

        public virtual IActionResult RedirectUserProfile(string id)
        {
            //we can't use dash in MVC
            int userId = Convert.ToInt32(id);
            var user = _customerService.GetCustomerById(userId);
            if (user == null)
                return RedirectToRoutePermanent("HomePage");

            return RedirectToRoutePermanent("CustomerProfile", new { id = user.Id });
        }

        #endregion
    }
}