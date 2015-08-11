using Orchard.ContentManagement.Records;

namespace Devq.Sellit.Models
{
    public class PriceChoicePartRecord : ContentPartRecord
    {
        public virtual PriceMode PriceMode { get; set; }
        public virtual decimal? Price { get; set; }
    }
}