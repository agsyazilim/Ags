using System.ComponentModel.DataAnnotations;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.News
{
    public partial class AddNewsCommentModel : BaseAgsModel
    {
        [Required(ErrorMessage = "Lütfen Yorum İçin Bir Başlık Giriniz")]
        [Display(Name = "Yorum Başlığı")]
        
        public string CommentTitle { get; set; }
        [Required(ErrorMessage = "Yorumunuzu Giriniz")]
        [Display(Name = "Yorum")]
        public string CommentText { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}