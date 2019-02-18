using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Blogs
{
    public partial class BlogPostYearModel : BaseAgsModel
    {
        public BlogPostYearModel()
        {
            Months = new List<BlogPostMonthModel>();
        }
        public int Year { get; set; }
        public IList<BlogPostMonthModel> Months { get; set; }
    }

    public partial class BlogPostMonthModel : BaseAgsModel
    {
        public int Month { get; set; }

        public int BlogPostCount { get; set; }
    }
}