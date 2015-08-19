using Devq.Sellit.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;

namespace Devq.Sellit.Handlers
{
    [OrchardFeature("Devq.FeaturedProducts")]
    public class FeaturedProductsWidgetHandler : ContentHandler
    {
        public FeaturedProductsWidgetHandler() {
            OnLoading<FeaturedProductsWidget>((ctx, part) => LoadLazyFields(part));
        }

        private void LoadLazyFields(FeaturedProductsWidget part) {
            // TODO: Load featured products
        }
    }
}