using Orchard.DisplayManagement.Descriptors;

namespace Devq.Sellit {
    public class ControlWrapper : IShapeTableProvider {
        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("Content").OnDisplaying(displaying => {
                if (displaying.Shape.ContentItem.ContentType == "Product"
                    && displaying.ShapeMetadata.DisplayType == "Detail") {
                    displaying.ShapeMetadata.Wrappers.Add("Product_EditWrapper");
                }
            });
        }
    }
}