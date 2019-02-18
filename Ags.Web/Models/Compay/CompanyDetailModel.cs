using System;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Compay
{
    public class CompanyDetailModel:BaseAgsEntityModel
    {
        
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Gsm { get; set; }
        public string Fax { get; set; }
        public string Www { get; set; }
        public string PictureUrl { get; set; }
        public string City { get; set; }
        public string VideoEmbedCode { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string Url { get; set; }
        public DateTime StartDate { get; set; }




    }
}