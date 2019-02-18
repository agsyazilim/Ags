using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.News
{
    public partial class NewsItemListModel : BaseAgsModel
    {
        public NewsItemListModel()
        {
            PagingFilteringContext = new NewsPagingFilteringModel();
            NewsItems = new List<NewsItemModel>();
        }


        public NewsPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<NewsItemModel> NewsItems { get; set; }
    }
}