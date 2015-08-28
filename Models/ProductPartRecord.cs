using System;
using Devq.Sellit.Models.Enums;
using Orchard.ContentManagement.Records;

namespace Devq.Sellit.Models
{
    public class ProductPartRecord : ContentPartRecord
    {
        public virtual DateTime? CreatedUtc { get; set; }
        public virtual ProductStatus Status { get; set; }
        public virtual string Category { get; set; }
    }
}