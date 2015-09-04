using System.Linq;
using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace Devq.Sellit.Drivers
{
    [OrchardFeature("Devq.FeaturedProducts")]
    public class FeaturedProductsWidgetDriver : ContentPartDriver<FeaturedProductsWidget> {
        private readonly IContentManager _contentManager;
        public FeaturedProductsWidgetDriver(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        protected override DriverResult Display(FeaturedProductsWidget part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_FeaturedProductsWidget", () => {

                var list = shapeHelper.List();
                list.AddRange(part
                    .FeaturedProducts
                    .Select(p => _contentManager.BuildDisplay(p.Product, "Summary")));

                return shapeHelper.Parts_FeaturedProductsWidget(List: list);
            });
        }
    }
}