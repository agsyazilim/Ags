using System;
using Ags.Data.Domain;
using Ags.Data.Domain.Blogs;
using Ags.Data.Domain.Common;
using Ags.Services;
using Ags.Services.Blogs;
using Ags.Services.Common;
using Ags.Services.Customers;
using Ags.Web.Factories;
using Ags.Web.Models.Blogs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Controllers
{
    public class EditorController : BasePublicController
    {

        private readonly ICustomerBlogFactory _customerBlogFactory;
        private readonly IBlogService _blogService;
        private readonly INewsCounterService _newsCounterService;
        private readonly ICustomerService _customerService;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditorController(ICustomerBlogFactory customerBlogFactory, IBlogService blogService, INewsCounterService newsCounterService, UserManager<ApplicationUser> userManager, ICustomerService customerService)
        {
            _customerBlogFactory = customerBlogFactory;
            _blogService = blogService;
            _newsCounterService = newsCounterService;
            _userManager = userManager;
            _customerService = customerService;
        }


        public virtual IActionResult List(BlogPagingFilteringModel command, int editorId)
        {

            var model = _customerBlogFactory.PrepareBlogPostListModel(command, editorId);
            return View("List", model);
        }

        public virtual IActionResult EditorList()
        {
            var model = _customerBlogFactory.PrePareEditorListModel();
            return View(model);
        }
        public virtual IActionResult BlogByTag(BlogPagingFilteringModel command, int customerId)
        {
            var model = _customerBlogFactory.PrepareBlogPostListModel(command, customerId);
            return View("List", model);
        }
        public virtual IActionResult BlogByMonth(BlogPagingFilteringModel command, int customerId)
        {


            var model = _customerBlogFactory.PrepareBlogPostListModel(command, customerId);
            return View("List", model);
        }
        public virtual IActionResult BlogPost(int blogPostId)
        {
            var blogPost = _blogService.GetBlogPostById(blogPostId);
            if (blogPost == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new BlogPostModel();
            _customerBlogFactory.PrepareBlogPostModel(model, blogPost, true);

            return View(model);
        }
        [HttpPost]
        public virtual IActionResult BlogCommentAdd(int blogPostId, BlogPostModel model)
        {
            var form = model.Form;

            blogPostId = Convert.ToInt32(form["blogPostId"]);
            var blogPost = _blogService.GetBlogPostById(blogPostId);
            if (blogPost == null || !blogPost.AllowComments)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var comment = new BlogComment
                {
                    BlogPostId = blogPost.Id,
                    CustomerId = User.GetCustomer(_userManager,_customerService)==null?5:User.GetCustomer(_userManager,_customerService).Id,
                    CommentText = model.AddNewComment.CommentText,
                    IsApproved = false,
                    CreatedOnUtc = DateTime.UtcNow,
                };
                blogPost.BlogComments.Add(comment);
                _blogService.UpdateBlogPost(blogPost);

                //The text boxes should be cleared after a comment has been posted
                //That' why we reload the page
                TempData["ags.blog.addcomment.result"] = comment.IsApproved
                    ? "Yorumunuz Eklendi"
                    : "Onaylandıktan Sonra Eklenecektir.";
                return RedirectToAction("BlogPost", "Editor", new { blogPostId = blogPost.Id });
            }

            //If we got this far, something failed, redisplay form
            _customerBlogFactory.PrepareBlogPostModel(model, blogPost, true);
            return View(model);
        }
        protected void AdjustCount(int id)
        {
            var visitcounter = _newsCounterService.GetCounterNewsCounter(id, "BlogPost", DateTime.Now);
            if (visitcounter == null)
            {
                NewsCounter counter = new NewsCounter
                {
                    CreateDate = DateTime.Now,
                    EntityId = id,
                    EntityName = "BlogPost",
                    TotalVisitor = 1
                };
                _newsCounterService.Insert(counter);
            }
            else
            {
                visitcounter.TotalVisitor = visitcounter.TotalVisitor + 1;
                _newsCounterService.Update(visitcounter);
            }

        }
    }
}