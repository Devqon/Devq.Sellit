using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;
using Orchard.Taxonomies.Models;

namespace Devq.Sellit.Settings
{
    public class ExtraTermPartSettingsEvents : ContentDefinitionEditorEventsBase
    {
        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition) {
            if (definition.PartDefinition.Name != typeof (TermPart).Name) yield break;

            var settings = definition.Settings.GetModel<ExtraTermPartSettings>();

            yield return DefinitionTemplate(settings);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel) {
            if (builder.Name != typeof (TermPart).Name) yield break;

            var settings = new ExtraTermPartSettings();
            updateModel.TryUpdateModel(settings, typeof (ExtraTermPartSettings).Name, null, null);
            builder.WithSetting("ExtraTermPartSettings.OnlyDirectChildren", settings.OnlyDirectChildren.ToString());
        }
    }
}