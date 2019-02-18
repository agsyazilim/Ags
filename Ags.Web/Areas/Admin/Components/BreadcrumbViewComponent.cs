using System.Collections.Generic;
using Ags.Web.Areas.Admin.Models;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Components
{
    public class BreadcrumbViewComponent : AgsViewComponent
    {

        public BreadcrumbViewComponent()
        {

        }

        public IViewComponentResult Invoke(string filter)
        {
            if (ViewBag.Breadcrumb == null)
            {
                ViewBag.Breadcrumb = new List<Message>();
            }

            return View(ViewBag.Breadcrumb as List<Message>);
        }
    }
}
