using Devq.Sellit.Models;
using Orchard.ContentManagement.Drivers;

namespace Devq.Sellit.Drivers
{
    public class FeaturedProductPartDriver : ContentPartDriver<FeaturedProductPart> {
        protected override DriverResult Display(FeaturedProductPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_FeaturedProduct", () => {

                var shape = shapeHelper.Parts_FeaturedProduct(Part: part);

                if (displayType.Contains("Admin")) {
                    shape.Active(part.Active);
                }

                return shape;
            }),
            ContentShape("Parts_FeaturedProduct_Activate_SummaryAdmin", () => 
                shapeHelper.Parts_FeaturedProduct_Activate_SummaryAdmin(Active: part.Active)));
        }
    }
}