using System.Collections.Generic;
using Ags.Web.Framework.Models;
using Ags.Web.Models.Blogs;

namespace Ags.Web.Models.Customer
{
    public class CustomerBlogModel : BaseAgsModel
    {
        public CustomerBlogModel()
        {
            BlogPostModel = new BlogPostModel();
            AvailableBlogPostModels = new BlogPostListModel();
            BlogPostModels = new List<BlogPostModel>();


        }
        public string AvatarUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FaceBookLink { get; set; }
        public string InstagramLink { get; set; }
        public string TwitterLink { get; set; }
        public string Email { get; set; }
        public int EditorId { get; set; }
        public BlogPostModel BlogPostModel { get; set; }
        public List<BlogPostModel> BlogPostModels { get; set; }
        public BlogPostListModel AvailableBlogPostModels { get; set; }
    }
}