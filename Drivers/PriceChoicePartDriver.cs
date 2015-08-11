using Devq.Sellit.Models;
using Orchard.ContentManagement.Drivers;

namespace Devq.Sellit.Drivers
{
    public class PriceChoicePartDriver : ContentPartDriver<PriceChoicePart> {

        protected override DriverResult Display(PriceChoicePart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_PriceChoice", () => shapeHelper
                .Parts_PriceChoice(Part: part));
        }
    }
}