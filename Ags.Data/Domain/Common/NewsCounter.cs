using System;
using Ags.Data.Common;

namespace Ags.Data.Domain.Common
{
    public class NewsCounter:BaseEntity
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public int TotalVisitor { get; set; }
        public DateTime CreateDate { get; set; }

    }
}