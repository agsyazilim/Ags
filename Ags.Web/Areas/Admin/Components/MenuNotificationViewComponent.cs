using System.Collections.Generic;
using Ags.Web.Areas.Admin.Models;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Components
{
    public class MenuNotificationViewComponent : AgsViewComponent
    {

        public MenuNotificationViewComponent()
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
                FontAwesomeIcon = "fa fa-users text-aqua",
                ShortDesc = "5 new members joined today",
                URLPath = "#",
            });

            return messages;
        }
    }
}
