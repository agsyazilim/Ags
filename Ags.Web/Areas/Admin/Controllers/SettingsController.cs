using System;
using Ags.Data.Domain;
using Ags.Services.Configuration;
using Ags.Services.Media;
using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Areas.Admin.Models.Configuration;
using Ags.Web.Framework.Kendoui;
using Ags.Web.Framework.Mvc;
using Ags.Web.Framework.UI;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingsController : BaseAdminController
    {
        private readonly ISettingService _settingService;
        private readonly ISettingModelFactory _settingModelFactory;

        public SettingsController(ApplicationDbContext context,
            ISettingService settingService,
            IPictureService pictureService,
            ISettingModelFactory settingModelFactory, IPageHeadBuilder pageHeadBuilder)
        {
            _settingService = settingService;
            _settingModelFactory = settingModelFactory;
        }

        // GET: Admin/Settings
        public virtual ActionResult Index()
        {
            SettingSearchModel model = _settingModelFactory.PrepareSettingSearchModel(new SettingSearchModel());
            AddBreadcrumb("Ayarlar","/Admin/Settings/Index");
            AddPageHeader("Tüm Ayarlar","Ayarları Yapılandırın");
            return View(model);
        }
         [HttpPost]
        public virtual IActionResult Index(SettingSearchModel searchModel)
        {

            //prepare model
            SettingListModel model = _settingModelFactory.PrepareSettingListModel(searchModel);

            return Json(model);

        }

        [HttpPost]
        public virtual IActionResult SettingUpdate(SettingModel model)
        {


            if (model.Name != null)
            {
                model.Name = model.Name.Trim();
            }

            if (model.Value != null)
            {
                model.Value = model.Value.Trim();
            }

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }

            //try to get a setting with the specified id
            var setting = _settingService.GetSettingById(model.Id)
                ?? throw new ArgumentException("No setting found with the specified id");

            int storeId = model.StoreId;

            if (!setting.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase) ||
                setting.StoreId != storeId)
            {
                //setting name or store has been changed
                _settingService.DeleteSetting(setting);
            }

            _settingService.SetSetting(model.Name, model.Value, storeId);

            return new NullJsonResult();
        }
        [HttpPost]
        public virtual IActionResult SettingAdd(SettingModel model)
        {


            if (model.Name != null)
            {
                model.Name = model.Name.Trim();
            }

            if (model.Value != null)
            {
                model.Value = model.Value.Trim();
            }

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }

            int storeId = model.StoreId;
            _settingService.SetSetting(model.Name, model.Value, storeId);

            return new NullJsonResult();
        }
        [HttpPost]
        public virtual IActionResult SettingDelete(int id)
        {

            //try to get a setting with the specified id
            var setting = _settingService.GetSettingById(id)
                ?? throw new ArgumentException("No setting found with the specified id", nameof(id));

            _settingService.DeleteSetting(setting);
            return new NullJsonResult();
        }




        [HttpGet]
        public virtual IActionResult GeneralCommon()
        {
            //prepare model
            GeneralCommonSettingsModel model = _settingModelFactory.PrepareGeneralCommonSettingsModel();

            return View(model);

        }
        [HttpPost]
        public IActionResult GeneralCommon(GeneralCommonSettingsModel model)
        {
            StoreInformationSettings store = _settingService.LoadSetting<StoreInformationSettings>();
            store.StoreClosed = model.StoreInformationSettings.StoreClosed;
            store.LogoPictureId = model.StoreInformationSettings.LogoPictureId;
            store.HeaderBannerPictureId = model.StoreInformationSettings.HeaderBannerPictureId;
            store.WhiteLogoPictureId = model.StoreInformationSettings.WhiteLogoPictureId;
            //social pages
            store.FacebookLink = model.StoreInformationSettings.FacebookLink;
            store.TwitterLink = model.StoreInformationSettings.TwitterLink;
            store.YoutubeLink = model.StoreInformationSettings.YoutubeLink;
            store.GooglePlusLink = model.StoreInformationSettings.GooglePlusLink;
            store.InstagramLink = model.StoreInformationSettings.InstagramLink;
            store.MaximumImageSize = model.StoreInformationSettings.MaximumImageSize;
            store.MultipleThumbDirectories = model.StoreInformationSettings.MultipleThumbDirectories;
            store.PopupGridPageSize = model.StoreInformationSettings.PopupGridPageSize;
            store.RichEditorAdditionalSettings = model.StoreInformationSettings.RichEditorAdditionalSettings;
            store.RichEditorAllowStyleTag = model.StoreInformationSettings.RichEditorAllowStyleTag;
            store.RichEditorAllowJavaScript = model.StoreInformationSettings.RichEditorAllowJavaScript;
            store.UseIsoDateFormatInJsonResult = model.StoreInformationSettings.UseIsoDateFormatInJsonResult;
            store.UseNestedSetting = model.StoreInformationSettings.UseNestedSetting;
            store.AvatarPictureSize = model.StoreInformationSettings.AvatarPictureSize;
            store.ContactEmail = model.StoreInformationSettings.ContactEmail;
            store.DefaultGridPageSize = model.StoreInformationSettings.DefaultGridPageSize;
            store.DefaultImageQuality = model.StoreInformationSettings.DefaultImageQuality;
            store.DisplayMiniProfilerForAdminOnly = model.StoreInformationSettings.DisplayMiniProfilerForAdminOnly;
            store.CopyRigth = model.StoreInformationSettings.CopyRigth;

            _settingService.SaveSetting<StoreInformationSettings>(store, 0);
            SuccessNotification("Ayarlar Güncellendi");
            return RedirectToAction("GeneralCommon");

        }
    }
}
