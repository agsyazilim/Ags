using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public IActionResult Index()
        {
            AddPageHeader("Dashboard", "");
            return View();
        }

        [HttpPost]
        public IActionResult Index(object model)
        {
            return View("Index");
        }
        public IActionResult Error()
        {
            return View();
        }











    }
}
