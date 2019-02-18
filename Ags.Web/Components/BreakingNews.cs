using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class BreakingNewsViewComponent : AgsViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();

        }

    }
}