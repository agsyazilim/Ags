using Ags.Services.News;
using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class RigthSliderViewComponent : AgsViewComponent
    {
        public IViewComponentResult Invoke()
        {
           
            return View();

        }
    }
}