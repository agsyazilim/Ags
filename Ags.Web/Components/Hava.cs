using System.Linq;
using Ags.Data.Domain;
using Ags.Web.Framework.Components;
using Ags.Web.Infrastructure;
using Ags.Web.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Components
{
    /// <summary>
    /// doviz
    /// </summary>
    public class HavaViewComponent : AgsViewComponent
    {
        private readonly ApplicationDbContext _context;
        public HavaViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
           var sehirler = _context.StateProvinces.Where(x=>x.Name!="Ardahan").ToList();
           var model = new StateProvidenceModel();
           model.StateList = sehirler.Select(x => new SelectListItem
           {
               Text = x.Name.ToUpper(),
               Value = RemoveAccent.StringReplace(x.Name.ToUpper()),
               Selected = x.Name=="IZMIR"


           }).ToList();
           return View(model);
        }



    }
}