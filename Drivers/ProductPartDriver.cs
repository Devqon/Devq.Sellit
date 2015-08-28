using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;
using Orchard.Services;

namespace Devq.Sellit.Drivers
{
    public class ProductPartDriver : ContentPartDriver<ProductPart> {

        private readonly IClock _clock;
        private readonly IContentManager _contentManager;

        public ProductPartDriver(IClock clock, IContentManager contentManager) {
            _clock = clock;
            _contentManager = contentManager;
        }

        protected override DriverResult Display(ProductPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                // Product part
                ContentShape("Parts_Product", 
                    () => shapeHelper.Parts_Product()),
                // Author
                ContentShape("Parts_Author",
                    () => {
                        var user = part.As<CommonPart>().Owner;

                        var userDisplay = _contentManager.BuildDisplay(user.ContentItem, "Mini");

                        return shapeHelper.Parts_Author(Author: userDisplay);
                    }));
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