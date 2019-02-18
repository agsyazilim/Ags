using System;
using System.Linq;
using System.Threading.Tasks;
using Ags.Data.Domain;
using Ags.Data.Domain.Common;
using Ags.Data.Domain.News;
using Ags.Services;
using Ags.Services.Common;
using Ags.Services.Customers;
using Ags.Services.Media;
using Ags.Services.News;
using Ags.Services.Seo;
using Ags.Web.Areas.Admin.Models;
using Ags.Web.Factories;
using Ags.Web.Infrastructure;
using Ags.Web.Models.News;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Controllers
{
    public class NewsController : BasePublicController
    {
        private readonly INewsModelFactory _newsModelFactory;
        private readonly INewsService _newsService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly INewsCounterService _newsCounterService;
        private readonly ICustomerService _customerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPictureService _pictureService;
        public NewsController(INewsModelFactory newsModelFactory, INewsService newsService, IUrlRecordService urlRecordService, INewsCounterService newsCounterService, ICustomerService customerService, UserManager<ApplicationUser> userManager, IPictureService pictureService)
        {
            _newsModelFactory = newsModelFactory;
            _newsService = newsService;
            _urlRecordService = urlRecordService;
            _newsCounterService = newsCounterService;
            _customerService = customerService;
            _userManager = userManager;
            _pictureService = pictureService;
        }

        [HttpGet("haberdetay/{id}/{title}")]
        public IActionResult Index(int id, string title)
        {
            var haber = _newsService.GetNewsById(id);
            AdjustCount(id = haber.Id);
            var model = _newsModelFactory.PrepareNewsItemModel(new NewsItemModel(), haber, true);
            return View(model);
        }

        public IActionResult List(NewsPagingFilteringModel command)
        {
            var model = _newsModelFactory.PrePareNewsListModel(command);
            return View(model);
        }
        [HttpPost]
        public virtual IActionResult NewsCommentAdd(int newsItemId, NewsItemModel model, bool captchaValid)
        {
            var form = model.Form;
            newsItemId = Convert.ToInt32(form["newsItemId"]);
            var newsItem = _newsService.GetNewsById(newsItemId);
            if (newsItem == null || !newsItem.Published || !newsItem.AllowComments)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                var comment = new NewsComment
                {
                    NewsItemId = newsItem.Id,
                    CustomerId = User.GetCustomer(_userManager, _customerService) == null ? 5 : User.GetCustomer(_userManager, _customerService).Id,
                    CommentTitle = model.AddNewComment.CommentTitle,
                    CommentText = model.AddNewComment.CommentText,
                    IsApproved = false,
                    CreatedOnUtc = DateTime.UtcNow,
                };
                newsItem.NewsComments.Add(comment);
                _newsService.UpdateNews(newsItem);

                //The text boxes should be cleared after a comment has been posted
                //That' why we reload the page
                TempData["ags.news.addcomment.result"] = comment.IsApproved
                    ? "Yorum Eklendi" : "Onaydan Sonra Yorum Gözükecektir.";

                return RedirectToAction("Index", "News", new { id = newsItem.Id, title = _urlRecordService.GetSeName(newsItem) });
            }

            //If we got this far, something failed, redisplay form
            model = _newsModelFactory.PrepareNewsItemModel(model, newsItem, true);
            return View(model);
        }
        protected void AdjustCount(int id)
        {
            var visitcounter = _newsCounterService.GetCounterNewsCounter(id, "NewsItem", DateTime.Now);
            if (visitcounter == null)
            {
                NewsCounter counter = new NewsCounter
                {
                    CreateDate = DateTime.Now,
                    EntityId = id,
                    EntityName = "NewsItem",
                    TotalVisitor = 1
                };
                _newsCounterService.Insert(counter);
            }
            else
            {
                visitcounter.TotalVisitor = visitcounter.TotalVisitor + 1;
                _newsCounterService.Update(visitcounter);
            }

        }
        [HttpPost]
        public IActionResult TopNews()
        {
            var topNews = _newsService.GetTopNews();
            var message = topNews.Select(x => new Message
            {
                ShortDesc = x.Short?.Chop("50"),
                URLPath = _urlRecordService.GetSeName(x),
                AvatarURL = _pictureService.GetPictureUrl(x.PictureId, 150),
                TimeSpan = x.CreatedOnUtc.ToShortTimeString(),
                Id = x.Id
            });
            return Json(message);

        }

        [HttpPost]
        public IActionResult PopulerNews()
        {
            var topNews = _newsService.GetPopulerNews();
            var message = topNews.Select(x => new Message
            {
                ShortDesc = x.Short?.Chop("50"),
                URLPath = _urlRecordService.GetSeName(x),
                AvatarURL = _pictureService.GetPictureUrl(x.PictureId, 150),
                TimeSpan = x.CreatedOnUtc.ToShortTimeString(),
                Id = x.Id
            });
            return Json(message);

        }


    }
}