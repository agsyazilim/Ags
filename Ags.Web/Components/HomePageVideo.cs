using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    /// <summary>
    /// HomePageVideoViewComponent
    /// </summary>
    public class HomePageVideoViewComponent : AgsViewComponent
    {
        /// <exclude />
        public IViewComponentResult Invoke()
        {
           return View();
        }
    }
}