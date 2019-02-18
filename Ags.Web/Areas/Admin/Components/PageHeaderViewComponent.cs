using System;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Components
{
    public class PageHeaderViewComponent : AgsViewComponent
    {

        public PageHeaderViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            Tuple<string, string> message;

            if (ViewBag.PageHeader == null)
            {
                message = Tuple.Create(string.Empty, string.Empty);
            }
            else
            {
                message = ViewBag.PageHeader as Tuple<string, string>;
            }
            return View(message);
        }
    }
}
