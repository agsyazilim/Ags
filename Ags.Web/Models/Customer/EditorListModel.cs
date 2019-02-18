using System.Collections.Generic;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Customer
{
    public class EditorListModel : BaseAgsModel
    {
        public EditorListModel()
        {
            EditorModels = new List<EditorModel>();
           
        }

        public List<EditorModel> EditorModels { get; set; }
        

    }
}