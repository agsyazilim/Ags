using System.Linq;
using Ags.Services.Media;
using Ags.Services.News;
using Ags.Services.Seo;
using Ags.Web.Areas.Admin.Models;
using Ags.Web.Framework.Components;
using Ags.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class TopNewsViewComponent: AgsViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}