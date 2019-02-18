using System;
using System.Linq;
using AdminLTE.Areas.Admin.Factories;
using Ags.Data.Domain.Blogs;
using Ags.Data.Html;
using Ags.Services.Blogs;
using Ags.Services.Stores;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Blogs;
using Ags.Web.Framework.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the blog model factory implementation
    /// </summary>
    public partial class BlogModelFactory : IBlogModelFactory
    {
        #region Fields

        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IBlogService _blogService;
        private readonly IStoreService _storeService;

        #endregion

        #region Ctor

        public BlogModelFactory(IBaseAdminModelFactory baseAdminModelFactory,
            IBlogService blogService,
            IStoreService storeService)
        {
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._blogService = blogService;
            this._storeService = storeService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare blog content model
        /// </summary>
        /// <param name="blogContentModel">Blog content model</param>
        /// <param name="filterByBlogPostId"></param>
        /// <param name="customerId"></param>
        /// <returns>Blog content model</returns>
        public virtual BlogContentModel PrepareBlogContentModel(BlogContentModel blogContentModel, int? filterByBlogPostId,int customerId=0)
        {
            if (blogContentModel == null)
                throw new ArgumentNullException(nameof(blogContentModel));
            BlogPost blogPost = null;
            if (customerId != 0)
            {
                var blog = _blogService.GetBlogPostById(filterByBlogPostId ?? 0);
                if (blog.CustomerId == customerId)
                    blogPost = blog;
                if (blogPost != null)
                {
                    PrepareBlogPostSearchModel(blogContentModel.BlogPosts);
                    blogPost = _blogService.GetBlogPostById(filterByBlogPostId ?? 0);
                    PrepareBlogCommentSearchModel(blogContentModel.BlogComments, blogPost);
                    return blogContentModel;
                }
              
            }
            //prepare nested search models
            PrepareBlogPostSearchModel(blogContentModel.BlogPosts);
            blogPost = _blogService.GetBlogPostById(filterByBlogPostId ?? 0);
            PrepareBlogCommentSearchModel(blogContentModel.BlogComments, blogPost);

            return blogContentModel;
        }

        /// <summary>
        /// Prepare blog post search model
        /// </summary>
        /// <param name="searchModel">Blog post search model</param>
        /// <returns>Blog post search model</returns>
        public virtual BlogPostSearchModel PrepareBlogPostSearchModel(BlogPostSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged blog post list model
        /// </summary>
        /// <param name="searchModel">Blog post search model</param>
        /// <param name="customerId"></param>
        /// <returns>Blog post list model</returns>
        public virtual BlogPostListModel PrepareBlogPostListModel(BlogPostSearchModel searchModel,int customerId=0)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            
            //get blog posts
            var blogPosts = _blogService.GetAllBlogPosts(customerId:customerId, showHidden: true,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            BlogPostListModel model = new BlogPostListModel
            {
                Data = blogPosts.Select(blogPost =>
                {
                    //fill in model values from the entity
                    BlogPostModel blogPostModel = blogPost.ToModel<BlogPostModel>();

                    //little performance optimization: ensure that "Body" is not returned
                    blogPostModel.Body = string.Empty;

                    //convert dates to the user time
                    if (blogPost.StartDateUtc.HasValue)
                        blogPostModel.StartDate = blogPost.StartDateUtc.Value;
                    if (blogPost.EndDateUtc.HasValue)
                        blogPostModel.EndDate = blogPost.EndDateUtc.Value;
                    blogPostModel.CreatedOn = blogPost.CreatedOnUtc;

                    //fill in additional values (not existing in the entity)
                    blogPostModel.ApprovedComments = _blogService.GetBlogCommentsCount(blogPost, isApproved: true);
                    blogPostModel.NotApprovedComments = _blogService.GetBlogCommentsCount(blogPost, isApproved: false);

                    return blogPostModel;
                }),
                Total = blogPosts.TotalCount
            };

            return model;
        }

        /// <summary>
        /// Prepare blog post model
        /// </summary>
        /// <param name="model">Blog post model</param>
        /// <param name="blogPost">Blog post</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Blog post model</returns>
        public virtual BlogPostModel PrepareBlogPostModel(BlogPostModel model, BlogPost blogPost, bool excludeProperties = false)
        {
            //fill in model values from the entity
            if (blogPost != null)
            {
                model = model ?? blogPost.ToModel<BlogPostModel>();
                model.StartDate = blogPost.StartDateUtc;
                model.EndDate = blogPost.EndDateUtc;
               
            }

            //set default values for the new model
            if (blogPost == null)
                model.AllowComments = true;
            _baseAdminModelFactory.PrepareEditorList(model.AvailableEditors,true,"Editor Seçiniz");
            return model;
        }

        /// <summary>
        /// Prepare blog comment search model
        /// </summary>
        /// <param name="searchModel">Blog comment search model</param>
        /// <param name="blogPost">Blog post</param>
        /// <returns>Blog comment search model</returns>
        public virtual BlogCommentSearchModel PrepareBlogCommentSearchModel(BlogCommentSearchModel searchModel, BlogPost blogPost)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare "approved" property (0 - all; 1 - approved only; 2 - disapproved only)
            searchModel.AvailableApprovedOptions.Add(new SelectListItem
            {
                Text = "Hepsi",
                Value = "0"
            });
            searchModel.AvailableApprovedOptions.Add(new SelectListItem
            {
                Text = "Onaylı",
                Value = "1"
            });
            searchModel.AvailableApprovedOptions.Add(new SelectListItem
            {
                Text = "Onaysız",
                Value = "2"
            });

            searchModel.BlogPostId = blogPost?.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged blog comment list model
        /// </summary>
        /// <param name="searchModel">Blog comment search model</param>
        /// <param name="blogPostId">Blog post ID</param>
        /// <returns>Blog comment list model</returns>
        public virtual BlogCommentListModel PrepareBlogCommentListModel(BlogCommentSearchModel searchModel, int? blogPostId)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get parameters to filter comments
            DateTime? createdOnFromValue = searchModel.CreatedOnFrom == null ? null
                : (DateTime?)searchModel.CreatedOnFrom.Value;
            DateTime? createdOnToValue = searchModel.CreatedOnTo == null ? null
                : (DateTime?)searchModel.CreatedOnTo.Value.AddDays(1);
            bool? isApprovedOnly = searchModel.SearchApprovedId == 0 ? null : searchModel.SearchApprovedId == 1 ? true : (bool?)false;

            //get comments
            System.Collections.Generic.IList<BlogComment> comments = _blogService.GetAllComments(blogPostId: blogPostId,
                approved: isApprovedOnly,
                fromUtc: createdOnFromValue,
                toUtc: createdOnToValue,
                commentText: searchModel.SearchText);

            //prepare store names (to avoid loading for each comment)
            System.Collections.Generic.Dictionary<int, string> storeNames = _storeService.GetAllStores().ToDictionary(store => store.Id, store => store.Name);

            //prepare list model
            BlogCommentListModel model = new BlogCommentListModel
            {
                Data = comments.PaginationByRequestModel(searchModel).Select(blogComment =>
                {
                    //fill in model values from the entity
                    BlogCommentModel commentModel = blogComment.ToModel<BlogCommentModel>();

                    //fill in additional values (not existing in the entity)
                    commentModel.CustomerInfo = blogComment.Customer.Email;
                    commentModel.CreatedOn = blogComment.CreatedOnUtc;
                    commentModel.Comment = HtmlHelper.FormatText(blogComment.CommentText, false, true, false, false, false, false);

                    return commentModel;
                }),
                Total = comments.Count
            };

            return model;
        }

        #endregion
    }
}