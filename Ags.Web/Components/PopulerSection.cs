using System.Linq;
using Ags.Services.Catalog;
using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Ags.Web.Models.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class PopulerSectionViewComponent : AgsViewComponent
    {

        public IViewComponentResult Invoke()
        {

            return View();

        }
    }
}