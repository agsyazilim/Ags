using System.Collections.Generic;
using Ags.Data.Domain.Blogs;
using Ags.Web.Models.Blogs;
using Ags.Web.Models.Customer;

namespace Ags.Web.Factories
{
    public interface ICustomerBlogFactory
    {
        CustomerListModel PrepareCustomerListBlogModel();


        EditorListModel PrePareEditorListModel();
        CustomerBlogModel PrepareCustomerBlogModel(int customerId);
        CustomerBlogModel PrepareCustomerListBlogModel(int customerId);

        List<CustomerBlogModel> PrepareCustomerBlogModelList();
        /// <summary>
        /// Prepare blog comment model
        /// </summary>
        /// <param name="blogComment">Blog comment entity</param>
        /// <returns>Blog comment model</returns>
        BlogCommentModel PrepareBlogPostCommentModel(BlogComment blogComment);

        /// <summary>
        /// Prepare blog post model
        /// </summary>
        /// <param name="model">Blog post model</param>
        /// <param name="blogPost">Blog post entity</param>
        /// <param name="prepareComments">Whether to prepare blog comments</param>
        void PrepareBlogPostModel(BlogPostModel model, BlogPost blogPost, bool prepareComments);

        /// <summary>
        /// Prepare blog post list model
        /// </summary>
        /// <param name="command">Blog paging filtering model</param>
        /// <param name="id"></param>
        /// <returns>Blog post list model</returns>
        BlogPostListModel PrepareBlogPostListModel(BlogPagingFilteringModel command,int editorId);

        /// <summary>
        /// Prepare blog post tag list model
        /// </summary>
        /// <returns>Blog post tag list model</returns>
        BlogPostTagListModel PrepareBlogPostTagListModel();

        /// <summary>
        /// Prepare blog post year models
        /// </summary>
        /// <returns>List of blog post year model</returns>
        List<BlogPostYearModel> PrepareBlogPostYearModel();

        /// <summary>
        /// Prepare blog post model
        /// </summary>
        /// <param name="model">Blog post model</param>
        /// <param name="blogPost">Blog post entity</param>
        /// <param name="prepareComments">Whether to prepare blog comments</param>
        BlogPostModel PrepareBlogDetailPostModel(BlogPostModel model, BlogPost blogPost, bool prepareComments);
    }
}
