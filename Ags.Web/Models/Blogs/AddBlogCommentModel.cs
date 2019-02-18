using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Blogs
{
    public partial class AddBlogCommentModel : BaseAgsEntityModel
    {
        [Display(Name = "Yorum Giriniz")]
        [Required(ErrorMessage = "Lütfen Yorum Giriniz")]
        public string CommentText { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}