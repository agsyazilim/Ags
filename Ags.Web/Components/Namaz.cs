using System.Linq;
using Ags.Data.Domain;
using Ags.Web.Framework.Components;
using Ags.Web.Infrastructure;
using Ags.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Components
{
    public class NamazViewComponent : AgsViewComponent
    {

        private readonly ApplicationDbContext _context;

        public NamazViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(string sehirId, string ilceId)
        {
            var sehirler = _context.StateProvinces.ToList();
            var model = new NamazVakitModel();
            model.Sehirlers = sehirler.Select(x => new SelectListItem
            {
                Text = x.Name.ToUpper(),
                Value = RemoveAccent.StringReplace(x.Name.ToLower()),
                Selected = x.Name == "izmir"

            }).ToList();

            return View(model);
        }

        

    }
}