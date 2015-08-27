using Devq.Sellit.Models;
using Orchard.ContentManagement;

namespace Devq.Sellit.Services {
    public class ProductService : IProductService {

        public ContentPart GetNewExtensionPart(string category) {
            switch (category) {
                case "Vehicle":
                    return new VehiclePart{ Record = new VehiclePartRecord() };
            }
            return null;
        }
    }
}