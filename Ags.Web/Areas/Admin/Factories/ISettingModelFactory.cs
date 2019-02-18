using Ags.Web.Areas.Admin.Models.Configuration;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the setting model factory
    /// </summary>
    public partial interface ISettingModelFactory
    {

        /// <summary>
        /// Prepare general and common settings model
        /// </summary>
        /// <returns>General and common settings model</returns>
        GeneralCommonSettingsModel PrepareGeneralCommonSettingsModel();

        /// <summary>
        /// Prepare setting search model
        /// </summary>
        /// <param name="searchModel">Setting search model</param>
        /// <returns>Setting search model</returns>
        SettingSearchModel PrepareSettingSearchModel(SettingSearchModel searchModel);

        /// <summary>
        /// Prepare paged setting list model
        /// </summary>
        /// <param name="searchModel">Setting search model</param>
        /// <returns>Setting list model</returns>
        SettingListModel PrepareSettingListModel(SettingSearchModel searchModel);




    }
}