using System.Collections.Generic;
using Ags.Web.Framework.Models;
using Ags.Web.Models.Customer;

namespace Ags.Web.Models.Blogs
{
    public partial class BlogPostListModel : BaseAgsModel
    {
        public BlogPostListModel()
        {
            PagingFilteringContext = new BlogPagingFilteringModel();
            BlogPosts = new List<BlogPostModel>();
            EditorModel = new EditorModel();
        }

        public BlogPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<BlogPostModel> BlogPosts { get; set; }
        public EditorModel EditorModel { get; set; }
        
    }
}