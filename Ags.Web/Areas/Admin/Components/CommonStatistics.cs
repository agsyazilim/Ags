using Ags.Web.Areas.Admin.Factories;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Components
{
    public class CommonStatistics : AgsViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;

        public CommonStatistics(ICommonModelFactory commonModelFactory)
        {
            _commonModelFactory = commonModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _commonModelFactory.PrepareCommonStatisticsModel();
            return View(model);
        }
    }
}