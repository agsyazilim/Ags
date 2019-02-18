using System;
using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.News
{
    public partial class HomePageNewsItemsModel : BaseAgsModel, ICloneable
    {
        public HomePageNewsItemsModel()
        {
            NewsItems = new List<NewsItemModel>();
        }

        public IList<NewsItemModel> NewsItems { get; set; }

        public object Clone()
        {
            //we use a shallow copy (deep clone is not required here)
            return MemberwiseClone();
        }
    }
}