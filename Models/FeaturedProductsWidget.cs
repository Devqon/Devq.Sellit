using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using Orchard.Environment.Extensions;

namespace Devq.Sellit.Models
{
    [OrchardFeature("Devq.FeaturedProducts")]
    public class FeaturedProductsWidget : ContentPart {
        internal LazyField<IEnumerable<FeaturedProductPart>> _productsField = new LazyField<IEnumerable<FeaturedProductPart>>();

        public IEnumerable<FeaturedProductPart> FeaturedProducts
        {
            get { return _productsField.Value; }
        }
    }
}