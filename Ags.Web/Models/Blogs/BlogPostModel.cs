using System;
using System.Collections.Generic;
using Ags.Web.Framework.Models;
using Ags.Web.Models.Customer;

namespace Ags.Web.Models.Blogs
{
    public partial class BlogPostModel : BaseAgsEntityModel
    {
        public BlogPostModel()
        {
            Tags = new List<string>();
            Comments = new List<BlogCommentModel>();
            AddNewComment = new AddBlogCommentModel();

        }

        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }
        public int CustomerId { get; set; }
        public EditorModel EditorModel { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string BodyOverview { get; set; }
        public bool AllowComments { get; set; }
        public int NumberOfComments { get; set; }
        public int NumberOfRead { get; set; }
        public DateTime CreatedOn { get; set; }

        public IList<string> Tags { get; set; }

        public IList<BlogCommentModel> Comments { get; set; }
        public AddBlogCommentModel AddNewComment { get; set; }
    }
}