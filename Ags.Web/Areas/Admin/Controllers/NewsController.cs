using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Repository;
using Ags.Data.Domain;
using Ags.Data.Domain.Catalog;
using Ags.Data.Domain.News;
using Ags.Services.Catalog;
using Ags.Services.Customers;
using Ags.Services.Logging;
using Ags.Services.Media;
using Ags.Services.News;
using Ags.Services.Seo;
using Ags.Services.Stores;
using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Media.Galery;
using Ags.Web.Areas.Admin.Models.News;
using Ags.Web.Framework.Mvc;
using Ags.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public partial class NewsController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly INewsModelFactory _newsModelFactory;
        private readonly INewsService _newsService;
        private readonly IStoreService _storeService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerService _customerService;
        private readonly IRepository<NewsPictureMapping> _newsPictuRepository;
        private readonly IPictureService _pictureService;
        private readonly ICategoryService _categoryService;
        #endregion
        #region Ctor

        public NewsController(ICustomerActivityService customerActivityService,
            INewsModelFactory newsModelFactory,
            INewsService newsService,
            IStoreService storeService,
            IUrlRecordService urlRecordService,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager,
            ICustomerService customerService, IRepository<NewsPictureMapping> newsPictuRepository, IPictureService pictureService, ICategoryService categoryService)
        {
            this._customerActivityService = customerActivityService;
            this._newsModelFactory = newsModelFactory;
            this._newsService = newsService;
            this._storeService = storeService;
            this._urlRecordService = urlRecordService;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _customerService = customerService;
            _newsPictuRepository = newsPictuRepository;
            _pictureService = pictureService;
            _categoryService = categoryService;
        }

        #endregion
        #region Methods

        #region News items

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List(int? filterByNewsItemId)
        {

            //prepare model
            NewsContentModel model = _newsModelFactory.PrepareNewsContentModel(new NewsContentModel(), filterByNewsItemId);
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(NewsItemSearchModel searchModel)
        {
            //prepare model
            NewsItemListModel model = _newsModelFactory.PrepareNewsItemListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult NewsItemCreate()
        {


            //prepare model
            NewsItemModel model = _newsModelFactory.PrepareNewsItemModel(new NewsItemModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult NewsItemCreate(NewsItemModel model, bool continueEditing)
        {

            if (ModelState.IsValid)
            {

                NewsItem newsItem = model.ToEntity<NewsItem>();
                newsItem.StartDateUtc = model.StartDate;
                newsItem.EndDateUtc = model.EndDate;
                newsItem.CreatedOnUtc = DateTime.UtcNow;
                newsItem.PictureId = model.PictureId;
                newsItem.CustomerId = model.CustomerId == 0 ? 0 : model.CustomerId;
                _newsService.InsertNews(newsItem);

                if (model.SelectedCategoryIds.Any())
                {
                    foreach (int categoryId in model.SelectedCategoryIds)
                    {
                        newsItem.CategoryNews.Add(new CategoryNews
                        {
                            CategoryId = categoryId,
                            NewsId = newsItem.Id,
                            DisplayOrder = 1

                        });
                        _newsService.UpdateNews(newsItem);
                    }
                }
                //activity log
                _customerActivityService.InsertActivity("AddNewNews", string.Format("AddNewNews{0}", newsItem.Id), newsItem);

                //search engine name
                string seName = _urlRecordService.ValidateSeName(newsItem, model.SeName, model.Title, true);
                _urlRecordService.SaveSlug(newsItem, seName);
                SuccessNotification("Haber Eklendi");

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("NewsItemEdit", new { id = newsItem.Id });
            }

            //prepare model
            model = _newsModelFactory.PrepareNewsItemModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult NewsItemEdit(int id)
        {


            //try to get a news item with the specified id
            NewsItem newsItem = _newsService.GetNewsById(id);
            if (newsItem == null)
                return RedirectToAction("List");

            //prepare model
            NewsItemModel model = _newsModelFactory.PrepareNewsItemModel(null, newsItem);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult NewsItemEdit(NewsItemModel model, bool continueEditing)
        {


            //try to get a news item with the specified id
            NewsItem newsItem = _newsService.GetNewsById(model.Id);
            if (newsItem == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                newsItem = model.ToEntity(newsItem);
                newsItem.StartDateUtc = model.StartDate;
                newsItem.EndDateUtc = model.EndDate;
                newsItem.PictureId = model.PictureId;
                _newsService.UpdateNews(newsItem);
                if (model.SelectedCategoryIds.Any())
                {
                    bool ctnews = newsItem.CategoryNews.Any(x => model.SelectedCategoryIds.Contains(x.CategoryId));
                    if (ctnews)
                    {
                        var catNewsList = _categoryService.GetNewsItemCategoriesByNewsId(newsItem.Id);
                        foreach (var categoryNews in catNewsList)
                        {
                            _categoryService.DeleteCategoryNews(categoryNews);
                        }

                    }
                    foreach (int categoryId in model.SelectedCategoryIds)
                    {

                        newsItem.CategoryNews.Add(new CategoryNews
                        {
                            CategoryId = categoryId,
                            NewsId = newsItem.Id,
                            DisplayOrder = 1

                        });
                        _newsService.UpdateNews(newsItem);
                    }
                }

                //activity log
                _customerActivityService.InsertActivity("EditNews",
                    string.Format("EditNews{0}", newsItem.Id), newsItem);

                //search engine name
                string seName = _urlRecordService.ValidateSeName(newsItem, model.SeName, model.Title, true);
                _urlRecordService.SaveSlug(newsItem, seName);



                SuccessNotification("Haber Güncellendi");

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("NewsItemEdit", new { id = newsItem.Id });
            }

            //prepare model
            model = _newsModelFactory.PrepareNewsItemModel(model, newsItem, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {

            //try to get a news item with the specified id
            NewsItem newsItem = _newsService.GetNewsById(id);

            if (newsItem == null)
                return RedirectToAction("List");
            bool ctnews = newsItem.CategoryNews.Any();
            if (ctnews)
            {
                var catNewsList = _categoryService.GetNewsItemCategoriesByNewsId(newsItem.Id);
                foreach (var categoryNews in catNewsList)
                {
                    _categoryService.DeleteCategoryNews(categoryNews);
                }

            }

            _newsService.DeleteNews(newsItem);

            //activity log
            _customerActivityService.InsertActivity("DeleteNews",
                string.Format("DeleteNews{0}", newsItem.Id), newsItem);

            SuccessNotification("Silinidi");

            return RedirectToAction("List");
        }

        #endregion

        #region Comments

        public virtual IActionResult Comments(int? filterByNewsItemId)
        {

            //try to get a news item with the specified id
            NewsItem newsItem = _newsService.GetNewsById(filterByNewsItemId ?? 0);
            if (newsItem == null && filterByNewsItemId.HasValue)
                return RedirectToAction("List");

            //prepare model
            NewsCommentSearchModel model = _newsModelFactory.PrepareNewsCommentSearchModel(new NewsCommentSearchModel(), newsItem);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Comments(NewsCommentSearchModel searchModel)
        {



            //prepare model
            NewsCommentListModel model = _newsModelFactory.PrepareNewsCommentListModel(searchModel, searchModel.NewsItemId);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult CommentUpdate(NewsCommentModel model)
        {


            //try to get a news comment with the specified id
            NewsComment comment = _newsService.GetNewsCommentById(model.Id)
                ?? throw new ArgumentException("No comment found with the specified id");

            bool previousIsApproved = comment.IsApproved;

            comment.IsApproved = model.IsApproved;
            _newsService.UpdateNews(comment.NewsItem);

            //activity log
            _customerActivityService.InsertActivity("EditNewsComment",
                string.Format("EditNewsComment{0}", comment.Id), comment);



            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult CommentDelete(int id)
        {


            //try to get a news comment with the specified id
            NewsComment comment = _newsService.GetNewsCommentById(id)
                ?? throw new ArgumentException("No comment found with the specified id", nameof(id));

            _newsService.DeleteNewsComment(comment);

            //activity log
            _customerActivityService.InsertActivity("DeleteNewsComment", string.Format("DeleteNewsComment{0}", comment.Id), comment);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult DeleteSelectedComments(ICollection<int> selectedIds)
        {

            if (selectedIds == null)
                return Json(new { Result = true });

            IList<NewsComment> comments = _newsService.GetNewsCommentsByIds(selectedIds.ToArray());

            _newsService.DeleteNewsComments(comments);

            //activity log
            foreach (NewsComment newsComment in comments)
            {
                _customerActivityService.InsertActivity("DeleteNewsComment",
                    string.Format("DeleteNewsComment{0}", newsComment.Id), newsComment);
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult ApproveSelected(ICollection<int> selectedIds)
        {

            if (selectedIds == null)
                return Json(new { Result = true });

            //filter not approved comments
            IEnumerable<NewsComment> newsComments = _newsService.GetNewsCommentsByIds(selectedIds.ToArray()).Where(comment => !comment.IsApproved);

            foreach (NewsComment newsComment in newsComments)
            {
                newsComment.IsApproved = true;
                _newsService.UpdateNews(newsComment.NewsItem);



                //activity log
                _customerActivityService.InsertActivity("EditNewsComment",
                    string.Format("EditNewsComment{0}", newsComment.Id), newsComment);
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult DisapproveSelected(ICollection<int> selectedIds)
        {

            if (selectedIds == null)
                return Json(new { Result = true });

            //filter approved comments
            IEnumerable<NewsComment> newsComments = _newsService.GetNewsCommentsByIds(selectedIds.ToArray()).Where(comment => comment.IsApproved);

            foreach (NewsComment newsComment in newsComments)
            {
                newsComment.IsApproved = false;
                _newsService.UpdateNews(newsComment.NewsItem);

                //activity log
                _customerActivityService.InsertActivity("EditNewsComment",
                    string.Format("EditNewsComment{0}", newsComment.Id), newsComment);
            }

            return Json(new { Result = true });
        }

        #endregion

        #region Photo
        [HttpPost]
        public virtual IActionResult GaleriPictureList(int id)
        {

            NewsItem galeri = _newsService.GetNewsById(id);
            if (galeri == null)
                throw new ArgumentException("No product picture found with the specified id");
            IOrderedQueryable<NewsPictureMapping> query = from pg in _newsPictuRepository.Table
                        where pg.NewsId == galeri.Id
                        orderby pg.DisplayOrder, pg.Id
                        select pg;

            List<NewsPictureMapping> gridData = query.ToList();
            GalleryPictureListModel model = new GalleryPictureListModel
            {
                Data = gridData.Select(x =>
                {
                    GalleryPictureModel galeriPicturemodel = new GalleryPictureModel
                    {
                        Id = x.Id,
                        DisplayOrder = x.DisplayOrder,
                        GaleriId = x.NewsId,
                        PictureId = x.PictureId

                    };
                    var picture = _pictureService.GetPictureById(galeriPicturemodel.PictureId) ??
                                  throw new Exception("Resim Yok");
                    galeriPicturemodel.OverrideAltAttribute = picture.AltAttribute;
                    galeriPicturemodel.OverrideTitleAttribute = picture.TitleAttribute;
                    galeriPicturemodel.PictureUrl = _pictureService.GetPictureUrl(picture);
                    return galeriPicturemodel;

                }),
                Total = query.Count()
            };

            return Json(model);
        }
        [HttpPost]
        public virtual IActionResult GaleriPictureUpdate(GalleryPictureModel model)
        {

            NewsPictureMapping galeriPicture = _newsPictuRepository.GetById(model.Id)
                                ?? throw new ArgumentException("No product picture found with the specified id");

            var picture = _pictureService.GetPictureById(galeriPicture.PictureId)
                          ?? throw new ArgumentException("No picture found with the specified id");

            _pictureService.UpdatePicture(picture.Id,
                _pictureService.LoadPictureBinary(picture),
                picture.MimeType,
                picture.SeoFilename,
                model.OverrideAltAttribute,
                model.OverrideTitleAttribute);

            galeriPicture.DisplayOrder = model.DisplayOrder;
            _newsPictuRepository.Update(galeriPicture);
            return new NullJsonResult();
        }
        [HttpPost]
        public virtual IActionResult GaleriPictureDelete(int id)
        {
            //try to get a product picture with the specified id
            NewsPictureMapping galeriPicture = _newsPictuRepository.GetById(id)
                                 ?? throw new ArgumentException("No product picture found with the specified id");
            int pictureId = galeriPicture.PictureId;
            _newsPictuRepository.Delete(galeriPicture);
            //try to get a picture with the specified id
           var picture = _pictureService.GetPictureById(pictureId)
                          ?? throw new ArgumentException("No picture found with the specified id");
            _pictureService.DeletePicture(picture);
            return new NullJsonResult();
        }
        [HttpPost]
        public virtual IActionResult GaleriPictureAdd(int pictureId, int displayOrder,
            string overrideAltAttribute, string overrideTitleAttribute, string url, int galeriId)
        {
            if (pictureId == 0)
                throw new ArgumentException();

            NewsItem galeri = _newsService.GetNewsById(galeriId)
                         ?? throw new ArgumentException("No product found with the specified id");
            IOrderedQueryable<NewsPictureMapping> query = from pg in _newsPictuRepository.Table
                        where pg.NewsId == galeriId
                        orderby pg.DisplayOrder, pg.Id
                        select pg;
            if (query.Any(p => p.PictureId == pictureId))
                return Json(new { Result = false });
            var picture = _pictureService.GetPictureById(pictureId)
                          ?? throw new ArgumentNullException("No Picture found with th spec id");
            _pictureService.UpdatePicture(picture.Id,
                                        _pictureService.LoadPictureBinary(picture),
                                        picture.MimeType,
                                        picture.SeoFilename,
                                        overrideAltAttribute,
                                        overrideTitleAttribute
                                    );
            _pictureService.SetSeoFilename(picture.Id, _pictureService.GetPictureSeName(galeri.Title));
            _newsPictuRepository.Insert(new NewsPictureMapping
            {
                NewsId = galeriId,
                DisplayOrder = displayOrder,
                PictureId = pictureId,
                Url = url
            });

            return Json(new { Result = true });
        }


        #endregion
        #endregion
    }
}