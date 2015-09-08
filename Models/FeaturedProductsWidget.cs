using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.ContentManagement.Utilities;
using Orchard.Environment.Extensions;

namespace Devq.Sellit.Models
{
    [OrchardFeature("Devq.FeaturedProducts")]
    public class FeaturedProductsWidget : ContentPart<FeaturedProductsWidgetPartRecord> {
        internal LazyField<IEnumerable<FeaturedProductPart>> _productsField = new LazyField<IEnumerable<FeaturedProductPart>>();

        public IEnumerable<FeaturedProductPart> FeaturedProducts
        {
            get { return _productsField.Value; }
        }

        public int NumberOfFeaturedProducts {
            get { return Retrieve(r => r.NumberOfFeaturedProducts); }
            set { Store(r => r.NumberOfFeaturedProducts, value); }
        }
    }

    public class FeaturedProductsWidgetPartRecord : ContentPartRecord {
        public virtual int NumberOfFeaturedProducts { get; set; }
    }
}