using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Blogs
{
    public partial class BlogPostTagModel : BaseAgsModel
    {
        public string Name { get; set; }

        public int BlogPostCount { get; set; }
    }
}