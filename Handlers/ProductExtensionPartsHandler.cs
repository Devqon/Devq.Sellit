using Devq.Sellit.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;

namespace Devq.Sellit.Handlers
{
    [OrchardFeature("Devq.ProductExtensions")]
    public class ProductExtensionPartsHandler : ContentHandler
    {
        public ProductExtensionPartsHandler(IRepository<VehiclePartRecord> vehicleRepository) {
            Filters.Add(StorageFilter.For(vehicleRepository));
        }
    }
}