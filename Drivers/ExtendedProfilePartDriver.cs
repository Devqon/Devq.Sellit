using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace Devq.Sellit.Drivers
{
    public class ExtendedProfilePartDriver : ContentPartDriver<ExtendedProfilePart> {
        protected override DriverResult Display(ExtendedProfilePart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_ExtendedProfile",
                () => shapeHelper.Parts_ExtendedProfile(ContentPart: part));
        }

        protected override DriverResult Editor(ExtendedProfilePart part, dynamic shapeHelper) {
            return ContentShape("Parts_ExtendedProfile_Edit", () => shapeHelper.EditorTemplate(
                TemplateName: "Parts.ExtendedProfile",
                Model: part,
                Prefix: Prefix));
        }

        protected override DriverResult Editor(ExtendedProfilePart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}