using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Models
{
    [ModelBinder(BinderType = typeof(Message))]
    public class Message
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string FontAwesomeIcon { get; set; }
        public string AvatarURL { get; set; }
        public string URLPath { get; set; }
        public string ShortDesc { get; set; }
        public string TimeSpan { get; set; }
        public int Percentage { get; set; }
        public string Type { get; set; }

        [UIHint("Boolean")]
        public bool Generic { get; set; }
    }
}
