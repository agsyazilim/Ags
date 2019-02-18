using Ags.Web.Framework.Models;

namespace Ags.Web.Areas.Admin.Models.Enews
{
    /// <summary>
    /// Represents a news content model
    /// </summary>
    public partial class ENewsContentModel : BaseAgsModel
    {
        #region Ctor

        public ENewsContentModel()
        {
            ENewsItemSearchModel = new ENewsItemSearchModel();
            ENewsCategoriesSearchModel = new ENewsCategoriesSearchModel();
        }

        #endregion

        #region Properties

        public ENewsItemSearchModel ENewsItemSearchModel { get; set; }

        public ENewsCategoriesSearchModel ENewsCategoriesSearchModel { get; set; }


        #endregion
    }
}
