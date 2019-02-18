using Ags.Web.Areas.Admin.Models;

namespace Ags.Web.Areas.Admin.Helpers
{
    /// <summary>
    /// This is where you customize the navigation sidebar
    /// </summary>
    public static class ModuleHelper
    {
        public enum Module
        {
            Dashboard,
            Kategoriler,
            Haberler,
            KoseYazilari,
            AnketYonetimi,
            EGazete,
            Topic,
            Reklam,
            FirmaKategori,
            Firma,
            Galeri,
            Video,
            Slider,
            Customer,
            Activity,
            GeneralSettings,
            AllSettings,
            EmailAccount,
            Stores,
            Log,
            Templates,
            Section,
            Modul,
            Noties

        }

        public static SidebarMenu AddHeader(string name)
        {
            return new SidebarMenu
            {
                Type = SidebarMenuType.Header,
                Name = name,
            };
        }

        public static SidebarMenu AddTree(string name, string iconClassName = "fa fa-link",bool permission=false)
        {
            return new SidebarMenu
            {
                Type = SidebarMenuType.Tree,
                IsActive = false,
                Name = name,
                IconClassName = iconClassName,
                URLPath = "#",
                Permissions = permission
            };
        }

        public static SidebarMenu AddModule(Module module)
        {
            switch (module)
            {
                case Module.Dashboard:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Kontrol Paneli",
                        IconClassName = "fa fa-desktop",
                        URLPath = "/Admin/Home/Index",
                    };
                case Module.Kategoriler:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Kategoriler",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Category/List",
                        Permissions = true
                    };
                case Module.Haberler:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Haberler",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/News/List",
                        Permissions = true
                    };
                case Module.KoseYazilari:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Köşe Yazıları",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Blog/List",
                        Permissions = true
                    };
                case Module.AnketYonetimi:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Anket Yönetimi",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Poll/List",
                        Permissions = true
                    };
                case Module.EGazete:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "E-Gazete",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/EnewsPapers/List",
                        Permissions = true
                    };
                case Module.Topic:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Sayfalar",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Topic/List",
                        Permissions = true
                    };
                case Module.Reklam:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Reklam",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Advertisements/Index",
                        Permissions = true
                    };
                case Module.FirmaKategori:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Firma Kategorisi",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/CompanyCategories/Index",
                        Permissions = true
                    };
                case Module.Firma:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Firma Yönetimi",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Companys/Index",
                        Permissions = true
                    };
                case Module.Galeri:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Galeri",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/PhotoGalleries/List",
                        Permissions = true
                    };
                case Module.Slider:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Slider",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Sliders/Index",
                        Permissions = true
                    };
                case Module.Video:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Video Yönetimi",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Videos/Index",
                        Permissions = true
                    };
                case Module.Customer:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Kullanıcı Listesi",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Customer/Index",
                        Permissions = true
                    };
                case Module.Activity:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Loglar",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/ActivityLog/List",
                        Permissions = true
                    };
                case Module.GeneralSettings:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Genel Ayarlar",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Settings/GeneralCommon",
                        Permissions = true
                    };
                case Module.AllSettings:
                    return new SidebarMenu
                    {

                        Type = SidebarMenuType.Link,
                        Name = "Tüm Ayarlar",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Settings/Index",
                        Permissions = true
                    };
                case Module.EmailAccount:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Mail Hesapları",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/EmailAccount/List",
                        Permissions = true
                    };
                case Module.Stores:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Site Bilgileri",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Stores/Index",
                        Permissions = true
                    };
                case Module.Log:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Log",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Log/List",
                        Permissions = true
                    };
                case Module.Templates:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Şablonlar",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Template/List",
                        Permissions = true
                    };
                case Module.Section:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Section Yönetimi",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Sections/Index",
                        Permissions = true
                    };
                case Module.Modul:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Modül Yönetimi",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Modules/Index",
                        Permissions = true
                    };
                case Module.Noties:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = "Duyurular",
                        IconClassName = "fa fa-dot-circle-o",
                        URLPath = "/Admin/Notices/Index",
                        Permissions = true
                    };
                default:
                    break;
            }

            return null;
        }
    }
}
