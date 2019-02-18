using System;
using System.Linq;
using Ags.Data.Domain;
using Ags.Services.Configuration;
using Ags.Services.Media;
using Ags.Web.Areas.Admin.Models.Configuration;
using Ags.Web.Framework.Extensions;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the setting model factory implementation
    /// </summary>
    public partial class SettingModelFactory : ISettingModelFactory
    {
        #region Fields

        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;

        #endregion

        #region Ctor

        public SettingModelFactory(

            IPictureService pictureService,
            ISettingService settingService)
        {
            this._pictureService = pictureService;
            this._settingService = settingService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare store information settings model
        /// </summary>
        /// <returns>Store information settings model</returns>
        protected virtual StoreInformationSettingsModel PrepareStoreInformationSettingsModel()
        {
            //load settings for a chosen store scope
            StoreInformationSettings storeInformationSettings = _settingService.LoadSetting<StoreInformationSettings>();

            //fill in model values from the entity
            StoreInformationSettingsModel model = new StoreInformationSettingsModel
            {
                StoreClosed = storeInformationSettings.StoreClosed,
                LogoPictureId = storeInformationSettings.LogoPictureId,
                WhiteLogoPictureId = storeInformationSettings.WhiteLogoPictureId,
                FacebookLink = storeInformationSettings.FacebookLink,
                TwitterLink = storeInformationSettings.TwitterLink,
                YoutubeLink = storeInformationSettings.YoutubeLink,
                GooglePlusLink = storeInformationSettings.GooglePlusLink,
                InstagramLink = storeInformationSettings.InstagramLink,
                HeaderBannerPictureId = storeInformationSettings.HeaderBannerPictureId,
                AvatarPictureSize = storeInformationSettings.AvatarPictureSize,
                ContactEmail = storeInformationSettings.ContactEmail,
                DefaultGridPageSize = storeInformationSettings.DefaultGridPageSize,
                DefaultImageQuality = storeInformationSettings.DefaultImageQuality,
                DisplayMiniProfilerForAdminOnly = storeInformationSettings.DisplayMiniProfilerForAdminOnly,
                DisplayMiniProfilerInPublicStore = storeInformationSettings.DisplayMiniProfilerInPublicStore,
                GridPageSizes = storeInformationSettings.GridPageSizes,
                MaximumImageSize = storeInformationSettings.MaximumImageSize,
                MultipleThumbDirectories = storeInformationSettings.MultipleThumbDirectories,
                PopupGridPageSize = storeInformationSettings.PopupGridPageSize,
                RichEditorAdditionalSettings = storeInformationSettings.RichEditorAdditionalSettings,
                RichEditorAllowJavaScript = storeInformationSettings.RichEditorAllowJavaScript,
                RichEditorAllowStyleTag = storeInformationSettings.RichEditorAllowStyleTag,
                UseIsoDateFormatInJsonResult = storeInformationSettings.UseIsoDateFormatInJsonResult,
                UseNestedSetting = storeInformationSettings.UseNestedSetting,
                UseRichEditorInMessageTemplates = storeInformationSettings.UseRichEditorInMessageTemplates,
                CopyRigth = storeInformationSettings.CopyRigth
               
                

            };
            return model;
        }


        #endregion

        #region Methods

        /// <summary>
        /// Prepare general and common settings model
        /// </summary>
        /// <returns>General and common settings model</returns>
        public virtual GeneralCommonSettingsModel PrepareGeneralCommonSettingsModel()
        {
            GeneralCommonSettingsModel model = new GeneralCommonSettingsModel();

            //prepare store information settings model
            model.StoreInformationSettings = PrepareStoreInformationSettingsModel();
            return model;
        }


        /// <summary>
        /// Prepare setting search model
        /// </summary>
        /// <param name="searchModel">Setting search model</param>
        /// <returns>Setting search model</returns>
        public virtual SettingSearchModel PrepareSettingSearchModel(SettingSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged setting list model
        /// </summary>
        /// <param name="searchModel">Setting search model</param>
        /// <returns>Setting list model</returns>
        public virtual SettingListModel PrepareSettingListModel(SettingSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get settings
           var settings = _settingService.GetAllSettings().AsQueryable();
            if (!string.IsNullOrEmpty(searchModel.SearchSettingName))
                settings = settings.Where(setting => setting.Name.ToLowerInvariant().Contains(searchModel.SearchSettingName.ToLowerInvariant()));
            if (!string.IsNullOrEmpty(searchModel.SearchSettingValue))
                settings = settings.Where(setting => setting.Value.ToLowerInvariant().Contains(searchModel.SearchSettingValue.ToLowerInvariant()));
            System.Collections.Generic.List<NewsSortingEnum> sortOptions = Enum.GetValues(typeof(NewsSortingEnum)).OfType<NewsSortingEnum>().ToList();
            //prepare list model
            SettingListModel model = new SettingListModel
            {
                Data = settings.PaginationByRequestModel(searchModel).Select(setting =>
                {
                    //fill in model values from the entity
                    SettingModel settingModel = new SettingModel
                    {
                        Id = setting.Id,
                        Name = setting.Name,
                        Value = setting.Value,
                        StoreId = setting.StoreId
                    };
                    return settingModel;
                }),

                Total = settings.Count()
            };

            return model;
        }



        #endregion
    }
}