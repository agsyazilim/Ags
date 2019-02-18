using System;
using Ags.Data.Domain;
using Ags.Data.Domain.Catalog;
using Ags.Services.Catalog;
using Ags.Services.Customers;
using Ags.Services.Logging;
using Ags.Services.Media;
using Ags.Services.Seo;
using Ags.Services.Stores;
using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Catalog;
using Ags.Web.Framework.Authorization;
using Ags.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public partial class CategoryController : BaseAdminController
    {
        #region Fields

        private readonly ICategoryModelFactory _categoryModelFactory;
        private readonly ICategoryService _categoryService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerService _customerService;
        #endregion

        #region Ctor

        public CategoryController(
            ICategoryModelFactory categoryModelFactory,
            ICategoryService categoryService,
            ICustomerActivityService customerActivityService,
            ICustomerService customerService,
            IPictureService pictureService,
            IStoreService storeService,
            IUrlRecordService urlRecordService,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager
            )
        {
            this._categoryModelFactory = categoryModelFactory;
            this._categoryService = categoryService;
            this._customerActivityService = customerActivityService;
            _customerService = customerService;
            this._urlRecordService = urlRecordService;
            this._authorizationService = authorizationService;
            this._userManager = userManager;
        }

        #endregion
        #region List

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            //var userId = GetCurrentUserAsync().Id; //_userManager.GetUserId(User);
            //var customer = _customerService.GetCustomerByAppId(userId);
            bool isAuthorized = _authorizationService.AuthorizeAsync(User,GetCurrentUserAsync(), CustomerOperations.Read).Result.Succeeded;
            if (!isAuthorized)
            {
                return AccessDeniedView();
            }

            //prepare model
            CategorySearchModel model = _categoryModelFactory.PrepareCategorySearchModel(new CategorySearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(CategorySearchModel searchModel)
        {
            bool isAuthorized = _authorizationService.AuthorizeAsync(User, GetCurrentUserAsync(), CustomerOperations.Read).Result.Succeeded;
            if (!isAuthorized)
            {
                return AccessDeniedView();
            }
            //prepare model
            CategoryListModel model = _categoryModelFactory.PrepareCategoryListModel(searchModel);

            return Json(model);
        }

        #endregion

        #region Create / Edit / Delete

        public virtual IActionResult Create()
        {
            bool isAuthorized = _authorizationService.AuthorizeAsync(User, GetCurrentUserAsync(), CustomerOperations.Create).Result.Succeeded;
            if (!isAuthorized)
            {
                return AccessDeniedView();
            }

            //prepare model
            CategoryModel model = _categoryModelFactory.PrepareCategoryModel(new CategoryModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(CategoryModel model, bool continueEditing)
        {
            bool isAuthorized = _authorizationService.AuthorizeAsync(User, GetCurrentUserAsync(), CustomerOperations.Create).Result.Succeeded;
            if (!isAuthorized)
            {
                return AccessDeniedView();
            }

            if (ModelState.IsValid)
            {
                Category category = model.ToEntity<Category>();
                category.CreatedOnUtc = DateTime.UtcNow;
                category.UpdatedOnUtc = DateTime.UtcNow;
                _categoryService.InsertCategory(category);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(category, model.SeName, category.Name,true);
                _urlRecordService.SaveSlug(category, model.SeName);
                _categoryService.UpdateCategory(category);

                //activity log
                _customerActivityService.InsertActivity("AddNewCategory",
                    string.Format("AddNewCategory{0}", category.Name), category);

                SuccessNotification("Kayıt Eklendi");

                if (!continueEditing)
                {
                    return RedirectToAction("List");
                }

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = category.Id });
            }

            //prepare model
            model = _categoryModelFactory.PrepareCategoryModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            bool isAuthorized = _authorizationService.AuthorizeAsync(User, GetCurrentUserAsync(), CustomerOperations.Update).Result.Succeeded;
            if (!isAuthorized)
            {
                return AccessDeniedView();
            }

            //try to get a category with the specified id
            Category category = _categoryService.GetCategoryById(id);
            if (category == null || category.Deleted)
            {
                return RedirectToAction("List");
            }

            //prepare model
            CategoryModel model = _categoryModelFactory.PrepareCategoryModel(null, category);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(CategoryModel model, bool continueEditing)
        {
            bool isAuthorized = _authorizationService.AuthorizeAsync(User, GetCurrentUserAsync(), CustomerOperations.Read).Result.Succeeded;
            if (!isAuthorized)
            {
                return AccessDeniedView();
            }

            //try to get a category with the specified id
            Category category = _categoryService.GetCategoryById(model.Id);
            if (category == null || category.Deleted)
            {
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {
                category = model.ToEntity(category);
                category.UpdatedOnUtc = DateTime.UtcNow;
                _categoryService.UpdateCategory(category);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(category, model.SeName, category.Name, true);
                _urlRecordService.SaveSlug(category, model.SeName);
                _categoryService.UpdateCategory(category);
                //activity log
                _customerActivityService.InsertActivity("EditCategory",
                    string.Format("EditCategory{0}", category.Name), category);

                SuccessNotification("Kayıt Güncellendi");

                if (!continueEditing)
                {
                    return RedirectToAction("List");
                }

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = category.Id });
            }

            //prepare model
            model = _categoryModelFactory.PrepareCategoryModel(model, category, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            bool isAuthorized = _authorizationService.AuthorizeAsync(User, GetCurrentUserAsync(), CustomerOperations.Delete).Result.Succeeded;
            if (!isAuthorized)
            {
                return AccessDeniedView();
            }

            //try to get a category with the specified id
            Category category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return RedirectToAction("List");
            }

            _categoryService.DeleteCategory(category);

            //activity log
            _customerActivityService.InsertActivity("DeleteCategory",
                string.Format("DeleteCategory{0}", category.Name), category);

            SuccessNotification("Kayıt Silindi");

            return RedirectToAction("List");
        }

        #endregion
    }
}