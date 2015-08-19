using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;
using Orchard.Environment.Extensions;

namespace Devq.Sellit.Models
{
    [OrchardFeature("Devq.FeaturedProducts")]
    public class FeaturedProductsWidget : ContentPart {
        internal LazyField<IEnumerable<IContent>> _productsField = new LazyField<IEnumerable<IContent>>();

        public IEnumerable<IContent> Products {
            get { return _productsField.Value; }
        }
    }
}