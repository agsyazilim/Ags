using System;
using Ags.Data.Domain;
using Ags.Data.Domain.News;
using Ags.Services;
using Ags.Services.Customers;
using Ags.Services.Events;
using Ags.Services.Logging;
using Ags.Services.NewsPapers;
using Ags.Services.Seo;
using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Enews;
using Ags.Web.Framework.Kendoui;
using Ags.Web.Framework.Mvc;
using Ags.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EnewsPapersController : BaseAdminController
    {
        private readonly INewsPaperServices _newsPaperServices;
        private readonly INewsPaperModelFactory _newsPaperModelFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUrlRecordService _urlRecordService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerService _customerService;
        private readonly ApplicationDbContext _applicationDbContext;



        public EnewsPapersController(INewsPaperServices newsPaperServices,
            INewsPaperModelFactory newsPaperModelFactory,
            IEventPublisher eventPublisher,
            IUrlRecordService urlRecordService,
            ICustomerActivityService customerActivityService,
            UserManager<ApplicationUser> userManager,
            ICustomerService customerService, ApplicationDbContext applicationDbContext)
        {
            _newsPaperServices = newsPaperServices;
            _newsPaperModelFactory = newsPaperModelFactory;
            _eventPublisher = eventPublisher;
            _urlRecordService = urlRecordService;
            _customerActivityService = customerActivityService;
            _userManager = userManager;
            _customerService = customerService;
            _applicationDbContext = applicationDbContext;
        }


        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {

            ENewsContentModel model = _newsPaperModelFactory.PrepareENewsContentModel(new ENewsContentModel());

            return View(model);
        }
         [HttpPost]
        public IActionResult List(ENewsItemSearchModel eNewsItemSearchModel)
         {
            ENewsItemListModel model = _newsPaperModelFactory.PrepareNewsItemListModel(eNewsItemSearchModel);

            return Json(model);
        }

        public IActionResult Create()
        {
            ENewsItemModel model = _newsPaperModelFactory.PrepareENewsItemModel(new ENewsItemModel(), null);

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Create(ENewsItemModel model,bool continueEditing)
        {

            if (ModelState.IsValid)
            {
                EnewsPaper newsItem = model.ToEntity<EnewsPaper>();
                newsItem.CreateDate = DateTime.Now;
                newsItem.NewsPaperCategoryId = model.SelectedCategoryId;
                newsItem.PictureId = model.PictureId;
                _newsPaperServices.InsertNews(newsItem);
                string seName = _urlRecordService.ValidateSeName(newsItem, model.SeName, model.Name, true);
                _urlRecordService.SaveSlug(newsItem,seName);
               _eventPublisher.EntityInserted(newsItem);
                SuccessNotification("E-gazete Eklenidi");

                _customerActivityService.InsertActivity(User.GetCustomer(_userManager, _customerService), "AddNewsPaper", "Yeni E-gazete Eklendi",newsItem);
                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = newsItem.Id });

            }

            return RedirectToAction("List");

        }

        public IActionResult Edit(int id)
        {
            EnewsPaper itemNews = _newsPaperServices.GetNewsById(id);
            ENewsItemModel model = _newsPaperModelFactory.PrepareENewsItemModel(new ENewsItemModel(),itemNews);

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Edit(ENewsItemModel model,bool continueEditing)
        {
            EnewsPaper newsItem = _newsPaperServices.GetNewsByIdAs(model.Id);
            if (newsItem == null)
                return RedirectToAction("List");
            if (ModelState.IsValid)
            {
                newsItem = model.ToEntity<EnewsPaper>();
                newsItem.CreateDate = DateTime.Now;
                newsItem.NewsPaperCategoryId = model.SelectedCategoryId;
                newsItem.PictureId = model.PictureId;
                _newsPaperServices.UpdateNews(newsItem);

                string seName = _urlRecordService.ValidateSeName(newsItem, model.SeName, model.Name, true);
                _urlRecordService.SaveSlug(newsItem, seName);
                //activity log
                _customerActivityService.InsertActivity("EditENewsPaper",string.Format("EditNewsPaper{0}", newsItem.Id), newsItem);

                //search engine name


                SuccessNotification("Güncelleme Yapıldı");

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = newsItem.Id });
            }

           return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            //try to get a news item with the specified id
            EnewsPaper newsItem = _newsPaperServices.GetNewsById(id);
            if (newsItem == null)
                return RedirectToAction("List");

            _newsPaperServices.DeleteNews(newsItem);

            //activity log
            _customerActivityService.InsertActivity("DeleteNewsPaper",string.Format("DeleteNewsPaper{0}", newsItem.Id), newsItem);

            SuccessNotification("Kayıt Silindi");

            return RedirectToAction("List");


        }
        [HttpPost]
        public IActionResult GetCategoriList(ENewsCategoriesSearchModel searchModel)
        {
            ENewsCategoriesListModel model = _newsPaperModelFactory.PrepareCategoriesListModel(searchModel);

            return Json(model);
        }
        [HttpPost]
        public IActionResult GetCategoriUpdate(ENewsCategoriesModel model)
        {
            if(model.Name != null)
            {
                model.Name = model.Name.Trim();
            }

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }
            _newsPaperServices.UpdateCategory(new NewsPaperCategory{Id = model.Id,Name = model.Name});
            SuccessNotification("Kategori Güncellendi.");

            return new NullJsonResult();
        }
         [HttpPost]
        public IActionResult GetCategoriCreate(ENewsCategoriesModel model)
        {
            if (model.Name != null)
            {
                model.Name = model.Name.Trim();
            }
            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }
            _newsPaperServices.InsertCategory(new NewsPaperCategory{Name = model.Name});
            SuccessNotification("Yeni Kategori Eklendi");
            return new NullJsonResult();
        }
         [HttpPost]
        public IActionResult GetCategoriDelete(int id)
         {
            NewsPaperCategory categori = _newsPaperServices.GetCategoriById(id);
             _newsPaperServices.DeleteNewsCategory(categori);
             SuccessNotification("Categori Silindi.");
             return new NullJsonResult();

        }



    }
}
