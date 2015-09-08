using Orchard.ContentManagement.Records;

namespace Devq.Sellit.Models
{
    public class LevelTermsWidgetPartRecord : ContentPartRecord
    {
        public virtual string Taxonomy { get; set; }
        public virtual string DisplayType { get; set; }
    }
}