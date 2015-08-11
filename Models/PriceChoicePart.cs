using Orchard.ContentManagement;

namespace Devq.Sellit.Models
{
    public class PriceChoicePart : ContentPart<PriceChoicePartRecord> {
        public PriceMode PriceMode {
            get { return Retrieve(r => r.PriceMode); }
            set { Store(r => r.PriceMode, value); }
        }

        public decimal? Price {
            get { return Retrieve(r => r.Price); }
            set { Store(r => r.Price, value); }
        }
    }
}