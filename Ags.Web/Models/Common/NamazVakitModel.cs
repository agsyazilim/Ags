using System.Collections.Generic;
using Ags.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ags.Web.Models.Common
{
    public class NamazVakitModel : BaseAgsModel
    {
        public NamazVakitModel()
        {
            Sehirlers = new List<SelectListItem>();
            VakitModel = new VakitModel();
        }
        public List<SelectListItem> Sehirlers { get; set; }
        public int SehirId { get; set; }
        public VakitModel VakitModel { get; set; }
    }
    public class VakitModel : BaseAgsModel
    {
        public string Gun { get; set; }//"Gun": "25.01.2019",

        public string Imsak { get; set; }//"Imsak": "06:17",

        public string Gunes { get; set; }//"Gunes": "07:40",

        public string Oglen { get; set; }//"Oglen": "12:56",

        public string Ikindi { get; set; }//"Ikindi": "15:39",

        public string Aksam { get; set; }//"Aksam": "18:02",

        public string Yatsi { get; set; }//"Yatsi": "19:20",

        public string Kible { get; set; }//"Kible": "11:56",

        public string Ogle { get; set; }//"Ogle": "12:56",

        public string MiladiTarihUzunIso8601 { get; set; }//"MiladiTarihUzunIso8601": "/Date(1548363600000+0300)/",
        public string KibleSaati { get; set; }//"KibleSaati": "11:56"
    }


}