using Ags.Services.Media;
using Ags.Services.News;
using Ags.Services.Seo;
using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class MainSliderViewComponent : AgsViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();

        }
    }
}