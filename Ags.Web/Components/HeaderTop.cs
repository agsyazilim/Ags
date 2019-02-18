using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class HeaderTopViewComponent:AgsViewComponent
    {
        public IViewComponentResult Invoke()
        {


            return View();
        }
    }
}