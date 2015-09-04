using Devq.Sellit.Models;
using Orchard.ContentManagement.Drivers;

namespace Devq.Sellit.Drivers
{
    public class FeaturedProductPartDriver : ContentPartDriver<FeaturedProductPart> {
        protected override DriverResult Display(FeaturedProductPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_FeaturedProduct", () => shapeHelper.Parts_FeaturedProduct(Part: part));
        }
    }
}