using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Ags.Web.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }
        public int AvatarId { get; set; }
        public string PictureUrl { get; set; }
    }
}
