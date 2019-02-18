using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Domain;
using Ags.Data.Domain.Blogs;
using Ags.Services;
using Ags.Services.Blogs;
using Ags.Services.Customers;
using Ags.Services.Logging;
using Ags.Services.Seo;
using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Areas.Admin.Infrastructure.Mappers.Extensions;
using Ags.Web.Areas.Admin.Models.Blogs;
using Ags.Web.Framework.Authorization;
using Ags.Web.Framework.Mvc;
using Ags.Web.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Controllers
{
    public partial class BlogController : BaseAdminController
    {
        #region Fields

        private readonly IBlogModelFactory _blogModelFactory;
        private readonly IBlogService _blogService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerService _customerService;

        #endregion

        #region Ctor

        public BlogController(IBlogModelFactory blogModelFactory,
            IBlogService blogService,
            ICustomerActivityService customerActivityService,
            IUrlRecordService urlRecordService,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager, ICustomerService customerService)
        {
            this._blogModelFactory = blogModelFactory;
            this._blogService = blogService;
            this._customerActivityService = customerActivityService;
            this._urlRecordService = urlRecordService;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _customerService = customerService;
        }

        #endregion



        #region Methods

        #region Blog posts

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }
        public virtual IActionResult List(int? filterByBlogPostId)
        {
            bool isAuthorized = _authorizationService.AuthorizeAsync(User, GetCurrentUserAsync(), CustomerOperations.Read).Result.Succeeded;
            if (!isAuthorized)
            {
                return AccessDeniedView();
            }
            //prepare model
            var model = _blogModelFactory.PrepareBlogContentModel(new BlogContentModel(), filterByBlogPostId);
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(BlogPostSearchModel searchModel)
        {
            bool isAuthorized = _authorizationService.AuthorizeAsync(User, GetCurrentUserAsync(), CustomerOperations.Read).Result.Succeeded;
            if (!isAuthorized)
            {
                return AccessDeniedView();
            }
            var customer = User.GetCustomer(_userManager, _customerService);
            BlogPostListModel model = null;
            if (customer.IsInCustomerRole(CustomerRole.Constants.CustomerManagersRole)&&!customer.IsInCustomerRole(CustomerRole
                    .Constants.CustomerAdministratorsRole))
            {
                model = _blogModelFactory.PrepareBlogPostListModel(searchModel, customer.Id);
                return Json(model);
            }
            //prepare model
            model = _blogModelFactory.PrepareBlogPostListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult BlogPostCreate()
        {
            bool isAuthorized = _authorizationService.AuthorizeAsync(User, GetCurrentUserAsync(), CustomerOperations.Create).Result.Succeeded;
            if (!isAuthorized)
            {
                return AccessDeniedView();
            }
            //prepare model
            BlogPostModel model = _blogModelFactory.PrepareBlogPostModel(new BlogPostModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult BlogPostCreate(BlogPostModel model, bool continueEditing)
        {
            bool isAuthorized = _authorizationService.AuthorizeAsync(User, GetCurrentUserAsync(), CustomerOperations.Create).Result.Succeeded;
            if (!isAuthorized)
            {
                return AccessDeniedView();
            }

            if (ModelState.IsValid)
            {
                BlogPost blogPost = model.ToEntity<BlogPost>();
                blogPost.StartDateUtc = model.StartDate;
                blogPost.EndDateUtc = model.EndDate;
                blogPost.CreatedOnUtc = DateTime.UtcNow;
                blogPost.CustomerId = model.CustomerId;

                _blogService.InsertBlogPost(blogPost);

                //activity log
                _customerActivityService.InsertActivity("AddNewBlogPost",
                    string.Format("AddNewBlogPost{0}", blogPost.Id), blogPost);

                //search engine name
                string seName = _urlRecordService.ValidateSeName(blogPost, model.SeName, model.Title, true);
                _urlRecordService.SaveSlug(blogPost, seName);
                SuccessNotification("Yazı Eklendi");

                if (!continueEditing)
                {
                    return RedirectToAction("List");
                }

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("BlogPostEdit", new { id = blogPost.Id });
            }

            //prepare model
            model = _blogModelFactory.PrepareBlogPostModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult BlogPostEdit(int id)
        {
            bool isAuthorized = _authorizationService.AuthorizeAsync(User, GetCurrentUserAsync(), CustomerOperations.Update).Result.Succeeded;
            if (!isAuthorized)
            {
                return AccessDeniedView();
            }


            //try to get a blog post with the specified id
            BlogPost blogPost = _blogService.GetBlogPostById(id);
            if (blogPost == null)
            {
                return RedirectToAction("List");
            }

            //prepare model
            BlogPostModel model = _blogModelFactory.PrepareBlogPostModel(null, blogPost);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult BlogPostEdit(BlogPostModel model, bool continueEditing)
        {
            bool isAuthorized = _authorizationService.AuthorizeAsync(User, GetCurrentUserAsync(), CustomerOperations.Update).Result.Succeeded;
            if (!isAuthorized)
            {
                return AccessDeniedView();
            }
            //try to get a blog post with the specified id
            BlogPost blogPost = _blogService.GetBlogPostById(model.Id);
            if (blogPost == null)
            {
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {
                blogPost = model.ToEntity(blogPost);
                blogPost.StartDateUtc = model.StartDate;
                blogPost.EndDateUtc = model.EndDate;
                blogPost.CustomerId = model.CustomerId;
                _blogService.UpdateBlogPost(blogPost);

                //activity log
                _customerActivityService.InsertActivity("EditBlogPost", string.Format("EditBlogPost{0}", blogPost.Id), blogPost);

                //search engine name
                string seName = _urlRecordService.ValidateSeName(blogPost, model.SeName, model.Title, true);
                _urlRecordService.SaveSlug(blogPost, seName);
                SuccessNotification("Güncelleme Yapıldı");

                if (!continueEditing)
                {
                    return RedirectToAction("List");
                }

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("BlogPostEdit", new { id = blogPost.Id });
            }

            //prepare model
            model = _blogModelFactory.PrepareBlogPostModel(model, blogPost, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            bool isAuthorized = _authorizationService.AuthorizeAsync(User, GetCurrentUserAsync(), CustomerOperations.Delete).Result.Succeeded;
            if (!isAuthorized)
            {
                return AccessDeniedView();
            }

            //try to get a blog post with the specified id
            BlogPost blogPost = _blogService.GetBlogPostById(id);
            if (blogPost == null)
            {
                return RedirectToAction("List");
            }

            _blogService.DeleteBlogPost(blogPost);

            //activity log
            _customerActivityService.InsertActivity("DeleteBlogPost",
                $"DeleteBlogPost{blogPost.Id}", blogPost);
            SuccessNotification("Deleted");
            return RedirectToAction("List");
        }

        #endregion

        #region Comments

        public virtual IActionResult Comments(int? filterByBlogPostId)
        {


            //try to get a blog post with the specified id
            BlogPost blogPost = _blogService.GetBlogPostById(filterByBlogPostId ?? 0);
            if (blogPost == null && filterByBlogPostId.HasValue)
            {
                return RedirectToAction("List");
            }

            //prepare model
            BlogCommentSearchModel model = _blogModelFactory.PrepareBlogCommentSearchModel(new BlogCommentSearchModel(), blogPost);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Comments(BlogCommentSearchModel searchModel)
        {


            //prepare model
            BlogCommentListModel model = _blogModelFactory.PrepareBlogCommentListModel(searchModel, searchModel.BlogPostId);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult CommentUpdate(BlogCommentModel model)
        {


            //try to get a blog comment with the specified id
            BlogComment comment = _blogService.GetBlogCommentById(model.Id)
                ?? throw new ArgumentException("No comment found with the specified id");

            bool previousIsApproved = comment.IsApproved;

            comment.IsApproved = model.IsApproved;
            _blogService.UpdateBlogPost(comment.BlogPost);
            //activity log
            _customerActivityService.InsertActivity("EditBlogComment",
                $"EditBlogComment{comment.Id}", comment);

            return new NullJsonResult();
        }

        public virtual IActionResult CommentDelete(int id)
        {


            //try to get a blog comment with the specified id
            BlogComment comment = _blogService.GetBlogCommentById(id)
                ?? throw new ArgumentException("No comment found with the specified id", nameof(id));

            _blogService.DeleteBlogComment(comment);

            //activity log
            _customerActivityService.InsertActivity("DeleteBlogPostComment",
                $"DeleteBlogPostComment{comment.Id}", comment);
            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult DeleteSelectedComments(ICollection<int> selectedIds)
        {


            if (selectedIds == null)
            {
                return Json(new { Result = true });
            }

            IList<BlogComment> comments = _blogService.GetBlogCommentsByIds(selectedIds.ToArray());

            _blogService.DeleteBlogComments(comments);
            //activity log
            foreach (BlogComment blogComment in comments)
            {
                _customerActivityService.InsertActivity("DeleteBlogPostComment",
                    $"DeleteBlogPostComment{blogComment.Id}", blogComment);
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult ApproveSelected(ICollection<int> selectedIds)
        {


            if (selectedIds == null)
            {
                return Json(new { Result = true });
            }

            //filter not approved comments
            IEnumerable<BlogComment> blogComments = _blogService.GetBlogCommentsByIds(selectedIds.ToArray()).Where(comment => !comment.IsApproved);

            foreach (BlogComment blogComment in blogComments)
            {
                blogComment.IsApproved = true;
                _blogService.UpdateBlogPost(blogComment.BlogPost);
                //activity log
                _customerActivityService.InsertActivity("EditBlogComment",
                    $"EditBlogComment{blogComment.Id}", blogComment);
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult DisapproveSelected(ICollection<int> selectedIds)
        {

            if (selectedIds == null)
            {
                return Json(new { Result = true });
            }

            //filter approved comments
            IEnumerable<BlogComment> blogComments = _blogService.GetBlogCommentsByIds(selectedIds.ToArray()).Where(comment => comment.IsApproved);

            foreach (BlogComment blogComment in blogComments)
            {
                blogComment.IsApproved = false;
                _blogService.UpdateBlogPost(blogComment.BlogPost);

                //activity log
                _customerActivityService.InsertActivity("EditBlogComment",
                    $"EditBlogComment{blogComment.Id}", blogComment);
            }

            return Json(new { Result = true });
        }

        #endregion

        #endregion
    }
}