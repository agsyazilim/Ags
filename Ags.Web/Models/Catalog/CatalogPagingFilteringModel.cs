using System.Collections.Generic;
using Ags.Web.Framework.UI.Paging;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Models.Catalog
{
    /// <summary>
    /// Filtering and paging model for catalog
    /// </summary>
    public partial class CatalogPagingFilteringModel : BasePageableModel
    {
        #region Ctor

        /// <summary>
        /// Constructor
        /// </summary>
        public CatalogPagingFilteringModel()
        {
            this.AvailableSortOptions = new List<SelectListItem>();
            this.AvailableViewModes = new List<SelectListItem>();
            this.PageSizeOptions = new List<SelectListItem>();

        }

        #endregion

        #region Properties





        /// <summary>
        /// A value indicating whether product sorting is allowed
        /// </summary>
        public bool AllowProductSorting { get; set; }

        /// <summary>
        /// Available sort options
        /// </summary>
        public IList<SelectListItem> AvailableSortOptions { get; set; }


        /// <summary>
        /// Available view mode options
        /// </summary>
        public IList<SelectListItem> AvailableViewModes { get; set; }


        /// <summary>
        /// Available page size options
        /// </summary>
        public IList<SelectListItem> PageSizeOptions { get; set; }

        /// <summary>
        /// Order by
        /// </summary>
        public int? OrderBy { get; set; }

        /// <summary>
        /// Product sorting
        /// </summary>
        public string ViewMode { get; set; }

        #endregion


    }
}