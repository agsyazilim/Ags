using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Common;
using Ags.Data.Core.Pages;
using Ags.Data.Domain;
using Ags.Data.Domain.Blogs;
using Ags.Data.Domain.Media;
using Ags.Services.Blogs;
using Ags.Services.Common;
using Ags.Services.Customers;
using Ags.Services.Media;
using Ags.Services.Seo;
using Ags.Web.Framework.Authorization;
using Ags.Web.Models.Blogs;
using Ags.Web.Models.Customer;
using Microsoft.AspNetCore.Identity;

namespace Ags.Web.Factories
{
    public class CustomerBlogFactory : ICustomerBlogFactory
    {
        private readonly IBlogService _blogservice;
        private readonly ICustomerService _customerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUrlRecordService _urlRecordService;
        private readonly BlogSettings _blogSettings;
        private readonly IStoreContext _storeContext;
        private readonly IPictureService _pictureService;
        private readonly INewsCounterService _newsCounterService;
       
       

        public CustomerBlogFactory(IBlogService blogService, ICustomerService customerService, UserManager<ApplicationUser> userManager, IUrlRecordService urlRecordService, BlogSettings blogSettings, IStoreContext storeContext, IPictureService pictureService, INewsCounterService newsCounterService)
        {
            _blogservice = blogService;
            _customerService = customerService;
            _userManager = userManager;
            _urlRecordService = urlRecordService;
            this._blogSettings = blogSettings;
            _storeContext = storeContext;
            _pictureService = pictureService;
            _newsCounterService = newsCounterService;
        }


        public CustomerListModel PrepareCustomerListBlogModel()
        {
            var model = new CustomerListModel {CustomerBlogList = PrepareCustomerBlogModelList()};

            return model;
           
        }

        public EditorListModel PrePareEditorListModel()
        {
            var model = new EditorListModel();
            var editorList = _customerService.GetRolesApplicationUsers(CustomerRole.Constants.CustomerManagersRole);
            foreach (var customer in editorList)
            {
                var appUser = _customerService.GetApplicationUserById(customer.OwnerId);

                var  editor = new EditorModel();
                editor.AvatarUrl = _pictureService.GetPictureUrl(Convert.ToInt32(_customerService.GetCustomerById(customer.Id).Zip),120,defaultPictureType:PictureType.Avatar); 
                editor.Email = appUser.Email;
                editor.FirstName = appUser.FirstName;
                editor.LastName = appUser.LastName;
                editor.EditorId = customer.Id;
                var claimsAsync = _customerService.GetUserClaim(customer.OwnerId);
                foreach (var item in claimsAsync)
                {
                    if (item.ClaimType == "FacebookLink")
                        editor.FaceBookLink = item.ClaimValue ?? "";
                    else if (item.ClaimType == "InstagramLink")
                        editor.InstagramLink = item.ClaimValue ?? "";
                    else if (item.ClaimType == "TwitterLink")
                        editor.TwitterLink = item.ClaimValue ?? "";
                }
                model.EditorModels.Add(editor);
                var blog = _blogservice.GetAllBlogPosts(customerId: editor.EditorId).LastOrDefault();
                var blogModel = new BlogPostModel();
                if (blog != null)
                {
                     PrepareBlogPostModel(blogModel, blog, false);
                }
               
                editor.BlogPostModel = blogModel;

            }
            return model;
        }

        public CustomerBlogModel PrepareCustomerListBlogModel(int customerId)
        {
            var model = new CustomerBlogModel();
            var customer = _customerService.GetCustomerById(customerId);
            var appUser = _customerService.GetApplicationUserById(customer.OwnerId);
            model.AvatarUrl = _pictureService.GetPictureUrl(
                    Convert.ToInt32(_customerService.GetCustomerById(customer.Id).Zip),120,defaultPictureType:PictureType.Avatar);
            model.Email = appUser.Email;
            model.FirstName = appUser.FirstName;
            model.LastName = appUser.LastName;
            model.EditorId = customerId;
            var claimsAsync = _customerService.GetUserClaim(customer.OwnerId);
            foreach (var item in claimsAsync)
            {
                if (item.ClaimType == "FacebookLink")
                    model.FaceBookLink = item.ClaimValue ?? "";
                else if (item.ClaimType == "InstagramLink")
                    model.InstagramLink = item.ClaimValue ?? "";
                else if (item.ClaimType == "TwitterLink")
                    model.TwitterLink = item.ClaimValue ?? "";
            }

            var blog = _blogservice.GetAllBlogPosts(customerId: customerId);
            foreach (var post in blog)
            {
                var blogModel = new BlogPostModel();
                PrepareBlogPostModel(blogModel, post, true);
                model.BlogPostModels.Add(blogModel);
            }
            model.AvailableBlogPostModels = PrepareBlogPostListModel(new BlogPagingFilteringModel
            {
                Tag = "",
                Month = "",
                PageSize = 0,
                PageNumber = 0
            },customerId);

            return model;
        }
        public CustomerBlogModel PrepareCustomerBlogModel(int customerId)
        {
            var model = new CustomerBlogModel();
            var customer = _customerService.GetCustomerById(customerId);
            var appUser = _customerService.GetApplicationUserById(customer.OwnerId);
            model.AvatarUrl = _pictureService.GetPictureUrl(
                    Convert.ToInt32(_customerService.GetCustomerById(customer.Id).Zip),120,defaultPictureType:PictureType.Avatar);
            model.Email = appUser.Email;
            model.FirstName = appUser.FirstName;
            model.LastName = appUser.LastName;
            model.EditorId = customerId;
            var claimsAsync = _customerService.GetUserClaim(customer.OwnerId);
            foreach (var item in claimsAsync)
            {
                if (item.ClaimType == "FacebookLink")
                    model.FaceBookLink = item.ClaimValue ?? "";
                else if (item.ClaimType == "InstagramLink")
                    model.InstagramLink = item.ClaimValue ?? "";
                else if (item.ClaimType == "TwitterLink")
                    model.TwitterLink = item.ClaimValue ?? "";
            }

            var blog = _blogservice.GetAllBlogPosts(customerId: customerId).FirstOrDefault();
            if (blog == null)
                return null;
            var blogModel = new BlogPostModel();
            PrepareBlogPostModel(blogModel, blog, true);
            model.BlogPostModel = blogModel;
            model.AvailableBlogPostModels = PrepareBlogPostListModel(new BlogPagingFilteringModel
            {
                Tag = "",
                Month = "",
                PageSize = 0,
                PageNumber = 0
            },customerId);

            return model;
        }

       

        public List<CustomerBlogModel> PrepareCustomerBlogModelList()
        {
            var model = new List<CustomerBlogModel>();
            var customerList = _customerService.GetRolesApplicationUsers(CustomerRole.Constants.CustomerManagersRole);
            foreach (var customer in customerList)
            {
                model.Add(PrepareCustomerBlogModel(customer.Id));
            }

            return model;
        }

        /// <summary>
        /// Prepare blog comment model
        /// </summary>
        /// <param name="blogComment">Blog comment entity</param>
        /// <returns>Blog comment model</returns>
        public virtual BlogCommentModel PrepareBlogPostCommentModel(BlogComment blogComment)
        {
            if (blogComment == null)
                throw new ArgumentNullException(nameof(blogComment));

            var model = new BlogCommentModel
            {
                Id = blogComment.Id,
                CustomerId = blogComment.CustomerId,
                CustomerName = blogComment.Customer.Name,
                CommentText = blogComment.CommentText,
                CreatedOn = blogComment.CreatedOnUtc,
            };

            return model;
        }

        /// <summary>
        /// Prepare blog post model
        /// </summary>
        /// <param name="model">Blog post model</param>
        /// <param name="blogPost">Blog post entity</param>
        /// <param name="prepareComments">Whether to prepare blog comments</param>
        public virtual void PrepareBlogPostModel(BlogPostModel model, BlogPost blogPost, bool prepareComments)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (blogPost == null)
                throw new ArgumentNullException(nameof(blogPost));

            model.Id = blogPost.Id;
            model.MetaTitle = blogPost.MetaTitle;
            model.MetaDescription = blogPost.MetaDescription;
            model.MetaKeywords = blogPost.MetaKeywords;
            model.SeName = _urlRecordService.GetSeName(blogPost);
            model.Title = blogPost.Title;
            model.Body = blogPost.Body;
            model.BodyOverview = blogPost.BodyOverview;
            model.AllowComments = blogPost.AllowComments;
            model.CreatedOn = blogPost.StartDateUtc ?? Convert.ToDateTime(blogPost.CreatedOnUtc);
            model.Tags = _blogservice.ParseTags(blogPost);
            model.NumberOfRead = _newsCounterService.GetByListCounter(blogPost.Id, "BlogPost", null).Sum(x=>x.TotalVisitor);
            //number of blog comments
            model.NumberOfComments = _blogservice.GetBlogCommentsCount(blogPost, _storeContext.CurrentStore.Id, true);

            if (prepareComments)
            {
                var blogComments = blogPost.BlogComments.Where(comment => comment.IsApproved);
                foreach (var bc in blogComments.OrderBy(comment => comment.CreatedOnUtc))
                {
                    var commentModel = PrepareBlogPostCommentModel(bc);
                    model.Comments.Add(commentModel);
                }
            }
            
            var customer = _customerService.GetCustomerById(blogPost.CustomerId);
            var appUser = _customerService.GetApplicationUserById(customer.OwnerId);
            var editorModel = new EditorModel
            {
                EditorId = customer.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                AvatarUrl = _pictureService.GetPictureUrl(
                    Convert.ToInt32(_customerService.GetCustomerById(customer.Id).Zip),120, defaultPictureType: PictureType.Avatar),
            Email = appUser.Email
            };
            var claimsAsync = _customerService.GetUserClaim(customer.OwnerId);
            foreach (var item in claimsAsync)
            {
                
                if (item.ClaimValue == "FacebookLink")
                    editorModel.FaceBookLink = item.ClaimValue ?? "";
                else if (item.ClaimType == "InstagramLink")
                    editorModel.InstagramLink = item.ClaimValue ?? "";
                else if (item.ClaimType == "TwitterLink")
                    editorModel.TwitterLink = item.ClaimValue ?? "";
            }

            model.EditorModel = editorModel;

        }

        /// <summary>
        /// Prepare blog post list model
        /// </summary>
        /// <param name="command">Blog paging filtering model</param>
       
        /// <param name="editorId"></param>
        /// <returns>Blog post list model</returns>
        public virtual BlogPostListModel PrepareBlogPostListModel(BlogPagingFilteringModel command,int editorId)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var model = new BlogPostListModel();
            model.PagingFilteringContext.Tag = command.Tag;
            model.PagingFilteringContext.Month = command.Month;

            if (command.PageSize <= 0) command.PageSize = _blogSettings.PostsPageSize;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            var dateFrom = command.GetFromMonth();
            var dateTo = command.GetToMonth();

            IPagedList<BlogPost> blogPosts;
            if (string.IsNullOrEmpty(command.Tag))
            {
                blogPosts = _blogservice.GetAllBlogPosts(customerId:editorId,dateFrom: dateFrom,dateTo: dateTo, pageIndex:command.PageNumber - 1, pageSize:command.PageSize);
            }
            else
            {
                blogPosts = _blogservice.GetAllBlogPostsByTag(editorId,command.Tag, command.PageNumber - 1, command.PageSize);
            }
            model.PagingFilteringContext.LoadPagedList(blogPosts);
            model.BlogPosts = blogPosts
                .Select(x =>
                {
                    var blogPostModel = new BlogPostModel();
                    PrepareBlogPostModel(blogPostModel, x, false);
                    return blogPostModel;
                })
                .ToList();

            var customer = _customerService.GetCustomerById(editorId);
            var appUser = _customerService.GetApplicationUserById(customer.OwnerId);
            model.EditorModel.AvatarUrl = _pictureService.GetPictureUrl(
                    Convert.ToInt32(_customerService.GetCustomerById(customer.Id).Zip),120, defaultPictureType: PictureType.Avatar);
            model.EditorModel.Email = appUser.Email;
            model.EditorModel.FirstName = appUser.FirstName;
            model.EditorModel.LastName = appUser.LastName;
           model.EditorModel.EditorId = customer.Id;
            var claimsAsync = _customerService.GetUserClaim(customer.OwnerId);
            foreach (var item in claimsAsync)
            {
                if (item.ClaimType == "FacebookLink")
                    model.EditorModel.FaceBookLink = item.ClaimValue ?? "";
                else if (item.ClaimType == "InstagramLink")
                    model.EditorModel.InstagramLink = item.ClaimValue ?? "";
                else if (item.ClaimType == "TwitterLink")
                    model.EditorModel.TwitterLink = item.ClaimValue ?? "";
            }
             

            return model;
        }

        /// <summary>
        /// Prepare blog post tag list model
        /// </summary>
        /// <returns>Blog post tag list model</returns>
        public virtual BlogPostTagListModel PrepareBlogPostTagListModel()
        {
            var model = new BlogPostTagListModel();

            //get tags
            var tags = _blogservice.GetAllBlogPostTags(_storeContext.CurrentStore.Id)
                .OrderByDescending(x => x.BlogPostCount)
                .Take(_blogSettings.NumberOfTags)
                .ToList();
            //sorting
            tags = tags.OrderBy(x => x.Name).ToList();

            foreach (var tag in tags)
                model.Tags.Add(new BlogPostTagModel
                {
                    Name = tag.Name,
                    BlogPostCount = tag.BlogPostCount
                });
            return model;

        }

        /// <summary>
        /// Prepare blog post year models
        /// </summary>
        /// <returns>List of blog post year model</returns>
        public virtual List<BlogPostYearModel> PrepareBlogPostYearModel()
        {
            var model = new List<BlogPostYearModel>();

            var blogPosts = _blogservice.GetAllBlogPosts(_storeContext.CurrentStore.Id);
            if (blogPosts.Any())
            {
                var months = new SortedDictionary<DateTime, int>();
                var blogPost = blogPosts[blogPosts.Count - 1];
                var first = blogPost.StartDateUtc ?? blogPost.CreatedOnUtc;
                while (DateTime.SpecifyKind(first, DateTimeKind.Utc) <= DateTime.UtcNow.AddMonths(1))
                {
                    var list = _blogservice.GetPostsByDate(blogPosts, new DateTime(first.Year, first.Month, 1),
                        new DateTime(first.Year, first.Month, 1).AddMonths(1).AddSeconds(-1));
                    if (list.Any())
                    {
                        var date = new DateTime(first.Year, first.Month, 1);
                        months.Add(date, list.Count);
                    }

                    first = first.AddMonths(1);
                }
                var current = 0;
                foreach (var kvp in months)
                {
                    var date = kvp.Key;
                    var blogPostCount = kvp.Value;
                    if (current == 0)
                        current = date.Year;

                    if (date.Year > current || !model.Any())
                    {
                        var yearModel = new BlogPostYearModel
                        {
                            Year = date.Year
                        };
                        model.Insert(0, yearModel);
                    }

                    model.First().Months.Insert(0, new BlogPostMonthModel
                    {
                        Month = date.Month,
                        BlogPostCount = blogPostCount
                    });

                    current = date.Year;
                }
            }
            return model;
        }

        public BlogPostModel PrepareBlogDetailPostModel(BlogPostModel model, BlogPost blogPost, bool prepareComments)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (blogPost == null)
                throw new ArgumentNullException(nameof(blogPost));

            model.Id = blogPost.Id;
            model.MetaTitle = blogPost.MetaTitle;
            model.MetaDescription = blogPost.MetaDescription;
            model.MetaKeywords = blogPost.MetaKeywords;
            model.SeName = _urlRecordService.GetSeName(blogPost);
            model.Title = blogPost.Title;
            model.Body = blogPost.Body;
            model.BodyOverview = blogPost.BodyOverview;
            model.AllowComments = blogPost.AllowComments;
            model.CreatedOn = blogPost.StartDateUtc ?? Convert.ToDateTime(blogPost.CreatedOnUtc);
            model.Tags = _blogservice.ParseTags(blogPost);
            //number of blog comments
            model.NumberOfComments = _blogservice.GetBlogCommentsCount(blogPost, _storeContext.CurrentStore.Id, true);
            var customer = _customerService.GetCustomerById(blogPost.CustomerId);
            var appUser = _customerService.GetApplicationUserById(customer.OwnerId);
            model.EditorModel.AvatarUrl = _pictureService.GetPictureUrl(
                    Convert.ToInt32(_customerService.GetCustomerById(customer.Id).Zip),120, defaultPictureType: PictureType.Avatar);
            model.EditorModel.Email = appUser.Email;
            model.EditorModel.FirstName = appUser.FirstName;
            model.EditorModel.LastName = appUser.LastName;
            model.EditorModel.EditorId = blogPost.CustomerId;
            var claimsAsync = _customerService.GetUserClaim(customer.OwnerId);
            foreach (var item in claimsAsync)
            {
                if (item.ClaimType == "FacebookLink")
                    model.EditorModel.FaceBookLink = item.ClaimValue ?? "";
                else if (item.ClaimType == "InstagramLink")
                    model.EditorModel.InstagramLink = item.ClaimValue ?? "";
                else if (item.ClaimType == "TwitterLink")
                    model.EditorModel.TwitterLink = item.ClaimValue ?? "";
            }

            if (prepareComments)
            {
                var blogComments = blogPost.BlogComments.Where(comment => comment.IsApproved);
                foreach (var bc in blogComments.OrderBy(comment => comment.CreatedOnUtc))
                {
                    var commentModel = PrepareBlogPostCommentModel(bc);
                    model.Comments.Add(commentModel);
                }
            }

            return model;

        }
    }
}
