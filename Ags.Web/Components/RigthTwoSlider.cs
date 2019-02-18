using Ags.Services.News;
using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class RigthTwoSliderViewComponent : AgsViewComponent
    {

        private readonly INewsService _newsService;
        private readonly INewsModelFactory _newsModelFactory;

        public RigthTwoSliderViewComponent(
            INewsService newsService, INewsModelFactory newsModelFactory)
        {
            _newsService = newsService;
            _newsModelFactory = newsModelFactory;
        }
        public IViewComponentResult Invoke()
        {

            return View();

        }
    }
}