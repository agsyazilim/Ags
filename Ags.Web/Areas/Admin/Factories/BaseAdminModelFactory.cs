using System;
using System.Collections.Generic;
using System.Linq;
using AdminLTE.Areas.Admin.Factories;
using Ags.Data.Core.Caching;
using Ags.Data.Domain;
using Ags.Data.Domain.Catalog;
using Ags.Data.Domain.Customers;
using Ags.Data.Domain.Directory;
using Ags.Data.Domain.Logging;
using Ags.Data.Domain.Media;
using Ags.Data.Domain.Message;
using Ags.Data.Domain.Stores;
using Ags.Data.Domain.Topics;
using Ags.Services;
using Ags.Services.Catalog;
using Ags.Services.Customers;
using Ags.Services.Directory;
using Ags.Services.Logging;
using Ags.Services.Media;
using Ags.Services.Message;
using Ags.Services.NewsPapers;
using Ags.Services.Stores;
using Ags.Services.Topics;
using Ags.Web.Areas.Admin.Helpers;
using Ags.Web.Framework.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the implementation of the base model factory that implements a most common admin model factories methods
    /// </summary>
    public partial class BaseAdminModelFactory : IBaseAdminModelFactory
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IStoreService _storeService;
        private readonly ITopicTemplateService _topicTemplateService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryTemplateService _categoryTemplateService;
        private readonly ICustomerService _customerService;
        private readonly INewsPaperServices _newsPaperServices;
        private readonly ISliderService _sliderService;
        private readonly IGalleryService _galleryService;
        private readonly IVideoService _videoService;

        #endregion

        #region Ctor

        public BaseAdminModelFactory(
            ICustomerActivityService customerActivityService,
            IStateProvinceService stateProvinceService,
            IStaticCacheManager cacheManager,
            IStoreService storeService,
            ITopicTemplateService topicTemplateService,
            IEmailAccountService emailAccountService,
            ICategoryService categoryService,
            ICategoryTemplateService categoryTemplateService, ICustomerService customerService, INewsPaperServices newsPaperServices,
             ISliderService sliderService, IGalleryService galleryService, IVideoService videoService)
        {
            this._customerActivityService = customerActivityService;
            this._stateProvinceService = stateProvinceService;
            this._cacheManager = cacheManager;
            this._storeService = storeService;
            this._topicTemplateService = topicTemplateService;
            _emailAccountService = emailAccountService;
            _categoryService = categoryService;
            _categoryTemplateService = categoryTemplateService;
            _customerService = customerService;
            _newsPaperServices = newsPaperServices;

            _sliderService = sliderService;
            _galleryService = galleryService;
            _videoService = videoService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare default item
        /// </summary>
        /// <param name="items">Available items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use "All" text</param>
        protected virtual void PrepareDefaultItem(IList<SelectListItem> items, bool withSpecialDefaultItem, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //whether to insert the first special item for the default value
            if (!withSpecialDefaultItem)
                return;

            //at now we use "0" as the default value
            const string value = "0";

            //prepare item text
            defaultItemText = defaultItemText ?? "Hepsi";

            //insert this default item at first
            items.Insert(0, new SelectListItem { Text = defaultItemText, Value = value });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare available activity log types
        /// </summary>
        /// <param name="items">Activity log type items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareActivityLogTypes(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available activity log types
            IList<ActivityLogType> availableActivityTypes = _customerActivityService.GetAllActivityTypes();
            foreach (ActivityLogType activityType in availableActivityTypes)
            {
                items.Add(new SelectListItem { Value = activityType.Id.ToString(), Text = activityType.Name });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }


        /// <summary>
        /// Prepare available states and provinces
        /// </summary>
        /// <param name="items">State and province items</param>
        /// <param name="countryId">Country identifier; pass null to don't load states and provinces</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareStatesAndProvinces(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available states and provinces of the country
            IList<StateProvince> availableStates = _stateProvinceService.GetStateProvincesByCountryId(showHidden: true);
            foreach (StateProvince state in availableStates)
            {
                items.Add(new SelectListItem { Value = state.Id.ToString(), Text = state.Name });
            }

            //insert special item for the default value
            if (items.Any())
                PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText ?? "SelectState");
            //insert special item for the default value
            if (!items.Any())
                PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText ?? "OtherNonUS");
        }



        /// <summary>
        /// Prepare available stores
        /// </summary>
        /// <param name="items">Store items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareStores(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available stores
            IList<Store> availableStores = _storeService.GetAllStores();
            foreach (Store store in availableStores)
            {
                items.Add(new SelectListItem { Value = store.Id.ToString(), Text = store.Name });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }



        /// <summary>
        /// Prepare available email accounts
        /// </summary>
        /// <param name="items">Email account items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareEmailAccounts(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available email accounts
            IList<EmailAccount> availableEmailAccounts = _emailAccountService.GetAllEmailAccounts();
            foreach (EmailAccount emailAccount in availableEmailAccounts)
            {
                items.Add(new SelectListItem { Value = emailAccount.Id.ToString(), Text = $"{emailAccount.DisplayName} ({emailAccount.Email})" });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }



        /// <summary>
        /// Prepare available categories
        /// </summary>
        /// <param name="items">Category items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareCategories(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available categories
            List<SelectListItem> availableCategoryItems = SelectListHelper.GetCategoryList(_categoryService, _cacheManager, true);
            foreach (SelectListItem categoryItem in availableCategoryItems)
            {
                items.Add(categoryItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PrepareNewsPaperCategories(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            IEnumerable<SelectListItem> availableCategorList = _newsPaperServices.GetAllCategories().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            foreach (SelectListItem listItem in availableCategorList)
            {
                items.Add(listItem);
            }

            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }


        /// <summary>
        /// Prepare available category templates
        /// </summary>
        /// <param name="items">Category template items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareCategoryTemplates(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available category templates
            IList<CategoryTemplate> availableTemplates = _categoryTemplateService.GetAllCategoryTemplates();
            foreach (CategoryTemplate template in availableTemplates)
            {
                items.Add(new SelectListItem { Value = template.Id.ToString(), Text = template.Name });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        /// <summary>
        /// Prepare available log levels
        /// </summary>
        /// <param name="items">Log level items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareLogLevels(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available log levels
            SelectList availableLogLevelItems = LogLevel.Debug.ToSelectList(false);
            foreach (SelectListItem logLevelItem in availableLogLevelItems)
            {
                items.Add(logLevelItem);
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }



        /// <summary>
        /// Prepare available topic templates
        /// </summary>
        /// <param name="items">Topic template items</param>
        /// <param name="withSpecialDefaultItem">Whether to insert the first special item for the default value</param>
        /// <param name="defaultItemText">Default item text; pass null to use default value of the default item text</param>
        public virtual void PrepareTopicTemplates(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            //prepare available topic templates
            IList<TopicTemplate> availableTemplates = _topicTemplateService.GetAllTopicTemplates();
            foreach (TopicTemplate template in availableTemplates)
            {
                items.Add(new SelectListItem { Value = template.Id.ToString(), Text = template.Name });
            }

            //insert special item for the default value
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PrepareEditorList(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            IList<Customer> availableEtitors = _customerService.GetRolesApplicationUsers(CustomerRole.Constants.CustomerManagersRole);
            foreach (Customer item in availableEtitors)
            {
                items.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PrepareSliders(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            var avilableSliders = _sliderService.GetAllSlider();
            foreach (Slider avilableSlider in avilableSliders)
            {
                items.Add(new SelectListItem { Value = avilableSlider.Id.ToString(), Text = avilableSlider.Name });
            }
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PreparePhotoGalleries(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
           var galleries = _galleryService.GetPhotoGalleries();
            foreach (PhotoGallery photoGallery in galleries)
            {
                items.Add(new SelectListItem { Value = photoGallery.Id.ToString(), Text = photoGallery.Name });
            }
            PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
        }

        public void PrepareVideoGalleries(IList<SelectListItem> items, bool withSpecialDefaultItem = true, string defaultItemText = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            var videoGalleries = _videoService.GetVideoGalleryList();
            foreach (VideoGallery item in videoGalleries)
            {
                items.Add(new SelectListItem
                { Value = item.Id.ToString(), Text = item.Name });
                PrepareDefaultItem(items, withSpecialDefaultItem, defaultItemText);
            }
        }

        #endregion
    }
}