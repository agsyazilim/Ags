using Ags.Web.Framework.Models;

namespace Ags.Web.Areas.Admin.Models.Common
{
    public partial class CommonStatisticsModel : BaseAgsModel
    {
        public int NumberOfNewsRead { get; set; }
        public int NumberOfCustomers { get; set; }
        public int NumberOfNotReadNews { get; set; }
        public int NumberOfReadBlogPost { get; set; }
        public int NumberOfNotReadBlogPost { get; set; }
        public int NumberOfDayVisitors { get; set; }
        public int NumberOfTotalVisitCount { get; set; }
    }
}