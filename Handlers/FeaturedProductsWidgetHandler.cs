using Devq.Sellit.Models;
using Devq.Sellit.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;

namespace Devq.Sellit.Handlers
{
    [OrchardFeature("Devq.FeaturedProducts")]
    public class FeaturedProductsWidgetHandler : ContentHandler {
        private readonly IFeaturedProductService _featuredProductService;
        private readonly IWorkContextAccessor _workContextAccessor;

        public FeaturedProductsWidgetHandler(IRepository<FeaturedProductsWidgetPartRecord> repository, IFeaturedProductService featuredProductService, IWorkContextAccessor workContextAccessor) {
            _featuredProductService = featuredProductService;
            _workContextAccessor = workContextAccessor;

            Filters.Add(StorageFilter.For(repository));

            OnLoading<FeaturedProductsWidget>((ctx, part) => LoadLazyFields(part));
        }

        private void LoadLazyFields(FeaturedProductsWidget part) {
            part._productsField.Loader(prt => {

                var toTake = part.NumberOfFeaturedProducts;
                if (toTake == 0) {
                    toTake = _workContextAccessor.GetContext().CurrentSite.As<FeaturedProductsSettingsPart>().NumberOfFeaturedProducts;
                }

                return _featuredProductService
                    .GetFeaturedProductsToday()
                    .Where<FeaturedProductPartRecord>(f => f.Active)
                    .Slice(0, toTake);
            });
        }
    }
}