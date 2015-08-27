using System.Linq;
using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Devq.Sellit.Handlers
{
    public class ProductExtensionsHandler : ContentHandler {
        private readonly IRepository<VehiclePartRecord> _vehicleRepository;
        private readonly IContentManager _contentManager;

        public ProductExtensionsHandler(IRepository<VehiclePartRecord> vehicleRepository, IContentManager contentManager) {
            _vehicleRepository = vehicleRepository;
            _contentManager = contentManager;
            Filters.Add(StorageFilter.For(vehicleRepository));

            OnLoading<ProductPart>(WeldExtensionPart);
        }

        private void WeldExtensionPart(LoadContentContext loadContentContext, ProductPart part) {
            var category = part.Category;

            if (string.IsNullOrEmpty(category))
                return;

            switch (category) {
                case "Vehicle":
                    var record = _vehicleRepository
                        .Table
                        .FirstOrDefault(t => t.Id == loadContentContext.ContentItem.Id);

                    if (record == null) break;
                    loadContentContext.ContentItem.Weld(new VehiclePart { Record = record });
                    break;
            }
        }
    }
}