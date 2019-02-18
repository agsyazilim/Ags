using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Customer
{
    public class CustomerListModel : BaseAgsModel
    {
        public CustomerListModel()
        {
            CustomerBlogList = new List<CustomerBlogModel>();
        }

        public List<CustomerBlogModel> CustomerBlogList { get; set; }

    }
}
