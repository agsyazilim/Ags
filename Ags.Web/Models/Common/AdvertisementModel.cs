using System;
using Ags.Web.Framework.Models;

namespace Ags.Web.Models.Common
{
    public partial class AdvertisementModel : BaseAgsEntityModel
    {
        public int PictureId { get; set; }
        public string PictureUrl { get; set; }
        public int SectionId { get; set; }
        public bool TargetId { get; set; }
        public string Target { get; set; }
        public string CodeFlash { get; set; }
        public string FlashCode { get; set; }
        public string UrlAddress { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsApproved { get; set; }

    }
}