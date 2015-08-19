using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Services;

namespace Devq.Sellit.Drivers
{
    public class ProductPartDriver : ContentPartDriver<ProductPart> {

        private readonly IClock _clock;

        public ProductPartDriver(IClock clock) {
            _clock = clock;
        }

        protected override DriverResult Display(ProductPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Product", () => shapeHelper.Parts_Product());
        }

        protected override DriverResult Editor(ProductPart part, dynamic shapeHelper) {
            return ContentShape("Parts_Product_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/Product",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(ProductPart part, IUpdateModel updater, dynamic shapeHelper) {
            if (updater.TryUpdateModel(part, Prefix, null, null)) {
                part.CreatedUtc = _clock.UtcNow;
            }

            return Editor(part, shapeHelper);
        }
    }
}