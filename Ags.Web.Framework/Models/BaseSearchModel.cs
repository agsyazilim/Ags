using Ags.Data.Core;
using Ags.Data.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Ags.Web.Framework.Models
{
    /// <summary>
    /// Represents base search model
    /// </summary>
    public abstract partial class BaseSearchModel : BaseAgsModel, IPagingRequestModel
    {
        #region Ctor

        public BaseSearchModel()
        {
            //set the default values
            this.Page = 1;
            this.PageSize = 10;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a page number
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets a page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a comma-separated list of available page sizes
        /// </summary>
        public string AvailablePageSizes { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Set grid page parameters
        /// </summary>
        public void SetGridPageSize()
        {
            StoreInformationSettings adminAreaSettings =ServiceProviderFactory.ServiceProvider.GetService<StoreInformationSettings>();

            this.Page = 1;
            this.PageSize = adminAreaSettings.DefaultGridPageSize;
            this.AvailablePageSizes = adminAreaSettings.GridPageSizes;
        }

        /// <summary>
        /// Set popup grid page parameters
        /// </summary>
        public void SetPopupGridPageSize()
        {
            StoreInformationSettings adminAreaSettings = ServiceProviderFactory.ServiceProvider.GetService<StoreInformationSettings>();

            this.Page = 1;
            this.PageSize = adminAreaSettings.PopupGridPageSize;
            this.AvailablePageSizes = adminAreaSettings.GridPageSizes;
        }

        #endregion
    }
}