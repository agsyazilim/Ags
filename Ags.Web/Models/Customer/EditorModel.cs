using Ags.Web.Framework.Models;
using Ags.Web.Models.Blogs;

namespace Ags.Web.Models.Customer
{
    public class EditorModel:BaseAgsModel
    {
        public EditorModel()
        {
            BlogPostModel = new BlogPostModel();
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
    }
}