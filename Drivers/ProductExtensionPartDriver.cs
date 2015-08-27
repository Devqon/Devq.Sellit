using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Devq.Sellit.Drivers
{
    public class ProductExtensionPartDriver : ContentPartDriver<ProductPart> {

        private readonly IContentManager _contentManager;

        public ProductExtensionPartDriver(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        protected override DriverResult Display(ProductPart part, string displayType, dynamic shapeHelper) {

            var category = part.Category;
            if (string.IsNullOrEmpty(category))
                return null;

            var partName = string.Format("Parts_{0}", category);

            var shape = _contentManager.BuildDisplay(_contentManager.New<VehiclePart>("Product"));
            if (shape == null)
                return null;

            return ContentShape(partName,
                () => shape);
        }
    }
}