using System.Collections.Generic;
using Ags.Services;
using Ags.Web.Areas.Admin.Helpers;
using Ags.Web.Areas.Admin.Models;
using Ags.Web.Framework.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Areas.Admin.Components
{
    public class SidebarViewComponent : AgsViewComponent
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public SidebarViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke(string filter)
        {

            var sidebars = new List<SidebarMenu>
            {
                ModuleHelper.AddModule(ModuleHelper.Module.Dashboard),
                ModuleHelper.AddModule(ModuleHelper.Module.KoseYazilari)
            };
            var haber = ModuleHelper.AddTree("Haber Yönetimi", "fa fa-book",HttpContext.User.IsAdmin());
            haber.TreeChild = new List<SidebarMenu>
            {
                ModuleHelper.AddModule(ModuleHelper.Module.Kategoriler),
                ModuleHelper.AddModule(ModuleHelper.Module.Haberler),
                ModuleHelper.AddModule(ModuleHelper.Module.AnketYonetimi),
                ModuleHelper.AddModule(ModuleHelper.Module.EGazete),

            };
            sidebars.Add(haber);
            var icerik = ModuleHelper.AddTree("İçerik Yönetimi", "fa fa-cubes",HttpContext.User.IsAdmin());
            sidebars.Add(icerik);
            var reklam = ModuleHelper.AddTree("Reklam Yönetimi", "fa fa-tags",HttpContext.User.IsAdmin());
            sidebars.Add(reklam);
            var ortam = ModuleHelper.AddTree("Ortam Yönetimi", "fa fa-gears",HttpContext.User.IsAdmin());
            sidebars.Add(ortam);
            var kullanici = ModuleHelper.AddTree("Kullanıcı Yönetimi", "fa fa-user-plus",HttpContext.User.IsAdmin());
            sidebars.Add(kullanici);
            var log = ModuleHelper.AddTree("Log Yönetimi", "fa fa-cube",HttpContext.User.IsAdmin());
            sidebars.Add(log);
            var setting = ModuleHelper.AddTree("Ayarlar", "fa fa-cogs",HttpContext.User.IsAdmin());
            sidebars.Add(setting);
            icerik.TreeChild = new List<SidebarMenu>()
            {
                ModuleHelper.AddModule(ModuleHelper.Module.Topic),
            };
            reklam.TreeChild = new List<SidebarMenu>()
            {
                ModuleHelper.AddModule(ModuleHelper.Module.Reklam),
                ModuleHelper.AddModule(ModuleHelper.Module.FirmaKategori),
                ModuleHelper.AddModule(ModuleHelper.Module.Firma),
            };
            ortam.TreeChild = new List<SidebarMenu>()
            {
                ModuleHelper.AddModule(ModuleHelper.Module.Galeri),
                ModuleHelper.AddModule(ModuleHelper.Module.Slider),
                ModuleHelper.AddModule(ModuleHelper.Module.Video),
            };

            kullanici.TreeChild = new List<SidebarMenu>()
            {
                ModuleHelper.AddModule(ModuleHelper.Module.Customer),
                ModuleHelper.AddModule(ModuleHelper.Module.EmailAccount),
            };

            log.TreeChild = new List<SidebarMenu>()
            {
                ModuleHelper.AddModule(ModuleHelper.Module.Activity),
                ModuleHelper.AddModule(ModuleHelper.Module.Log),
            };

            setting.TreeChild = new List<SidebarMenu>()
            {
                ModuleHelper.AddModule(ModuleHelper.Module.GeneralSettings),
                ModuleHelper.AddModule(ModuleHelper.Module.AllSettings),
                ModuleHelper.AddModule(ModuleHelper.Module.Stores),
                ModuleHelper.AddModule(ModuleHelper.Module.Templates),
                ModuleHelper.AddModule(ModuleHelper.Module.Section),
                ModuleHelper.AddModule(ModuleHelper.Module.Modul),
                ModuleHelper.AddModule(ModuleHelper.Module.Noties),
            };
            //sidebars.AddRange(settings);
            //XmlSiteMap siteMap = new XmlSiteMap();
            //siteMap.LoadFrom("/files/siteMap.config");

            return View(sidebars);
        }
    }
}
