using Devq.Sellit.Models;
using Devq.Sellit.Services;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;

namespace Devq.Sellit.Handlers
{
    [OrchardFeature("Devq.FeaturedProducts")]
    public class FeaturedProductsWidgetHandler : ContentHandler {
        private readonly IFeaturedProductService _featuredProductService;

        public FeaturedProductsWidgetHandler(IFeaturedProductService featuredProductService) {
            _featuredProductService = featuredProductService;

            OnLoading<FeaturedProductsWidget>((ctx, part) => LoadLazyFields(part));
        }

        private void LoadLazyFields(FeaturedProductsWidget part) {
            part._productsField.Loader(prt => {
                return _featuredProductService
                    .GetFeaturedProductsToday()
                    .Slice(0, 3);
            });
        }
    }
}