using Devq.Sellit.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Devq.Sellit.Handlers
{
    public class ProductExtensionPartsHandler : ContentHandler
    {
        public ProductExtensionPartsHandler(IRepository<VehiclePartRecord> vehicleRepository) {
            Filters.Add(StorageFilter.For(vehicleRepository));
        }
    }
}