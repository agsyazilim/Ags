using System;
using Ags.Data.Common;

namespace Ags.Data.Domain.Common
{
    public class VisitorCounter:BaseEntity
    {
        public int Count { get; set; }
        public DateTime CreateDate { get; set; }
    }
}