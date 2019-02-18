using System.Collections.Generic;
using System.Security.Claims;
using Ags.Data.Common;
using Ags.Services;
using Ags.Web.Areas.Admin.Models;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Components
{
    public class MenuMessageViewComponent : AgsViewComponent
    {

        public MenuMessageViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            List<Message> messages = GetData();
            return View(messages);
        }

        private List<Message> GetData()
        {
            List<Message> messages = new List<Message>();

            messages.Add(new Message
            {
                Id = 1,
                UserId = ((ClaimsPrincipal)User).GetUserProperty(CustomClaimTypes.NameIdentifier),
                DisplayName = "Support Team",
                AvatarURL = "/images/user.png",
                ShortDesc = "Why not buy a new awesome theme?",
                TimeSpan = "5 mins",
                URLPath = "#",
            });

            messages.Add(new Message
            {
                Id = 1,
                UserId = ((ClaimsPrincipal)User).GetUserProperty(CustomClaimTypes.NameIdentifier),
                DisplayName = "Ken",
                AvatarURL = "/images/user.png",
                ShortDesc = "For approval",
                TimeSpan = "15 mins",
                URLPath = "#",
            });

            return messages;
        }
    }
}
