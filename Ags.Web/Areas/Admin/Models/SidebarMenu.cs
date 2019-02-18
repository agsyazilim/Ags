using System.Collections.Generic;

namespace Ags.Web.Areas.Admin.Models
{
    public class SidebarMenu
    {
        public SidebarMenu()
        {
            TreeChild = new List<SidebarMenu>();
        }
        public SidebarMenuType Type { get; set; }
        public bool IsActive { get; set; } = false;
        public string Name { get; set; }
        public string IconClassName { get; set; }
        public string URLPath { get; set; }
        public List<SidebarMenu> TreeChild { get; set; }
        public bool Permissions { get; set; }

    }

    public enum SidebarMenuType
    {
        Header,
        Link,
        Tree
    }
}
