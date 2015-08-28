using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace Devq.Sellit.Drivers
{
    [OrchardFeature("Devq.ProductExtensions")]
    public class VehiclePartDriver : ContentPartDriver<VehiclePart>
    {
        protected override DriverResult Display(VehiclePart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_Vehicle", () => shapeHelper.Parts_Vehicle(Model: part));
        }

        protected override DriverResult Editor(VehiclePart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_Vehicle_Edit", () => shapeHelper.EditorTemplate(
                TemplateName: "ProductExtensions/Vehicle",
                Model: part,
                Prefix: Prefix));
        }

        protected override DriverResult Editor(VehiclePart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}