using System.Collections.Generic;
using Ags.Web.Areas.Admin.Models;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Components
{
    public class MenuTaskViewComponent : AgsViewComponent
    {

        public MenuTaskViewComponent()
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
                ShortDesc = "Design some buttons",
                URLPath = "#",
                Percentage = 20,
            });

            return messages;
        }
    }
}
