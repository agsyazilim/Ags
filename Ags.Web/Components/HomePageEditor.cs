using Ags.Services.Media;
using Ags.Web.Factories;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class HomePageEditorViewComponent : AgsViewComponent
    {
        private readonly IPictureService _pictureService;
        private readonly ICustomerBlogFactory _customerBlogFactory;
        public HomePageEditorViewComponent(IPictureService pictureService, ICustomerBlogFactory customerBlogFactory)
        {
            this._pictureService = pictureService;
            _customerBlogFactory = customerBlogFactory;

        }
        public IViewComponentResult Invoke()
        {
            var model = _customerBlogFactory.PrepareCustomerListBlogModel();

            return View(model);
        }
    }
}