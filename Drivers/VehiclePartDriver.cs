using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Devq.Sellit.Drivers
{
    public class VehiclePartDriver : ContentPartDriver<VehiclePart> {

        protected override DriverResult Display(VehiclePart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_Vehicle", () => shapeHelper.Parts_Vehicle());
        }

        protected override DriverResult Editor(VehiclePart part, dynamic shapeHelper) {
            return ContentShape("Parts_Vehicle_Edit", () => shapeHelper.EditorTemplate(
                TemplateName: "ProductExtensions/Vehicle",
                Model: part,
                Prefix: Prefix));
        }

        protected override DriverResult Editor(VehiclePart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}