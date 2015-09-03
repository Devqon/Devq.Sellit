using System.Collections.Generic;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;

namespace Devq.Sellit.Events
{
    public class ProductTypeEditorEvents : ContentDefinitionEditorEventsBase {
        private readonly string[] _invisibleParts = {"TitlePart"};

        public override IEnumerable<TemplateViewModel> TypeEditor(ContentTypeDefinition definition) {
            if (!definition.Settings.ContainsKey("Stereotype") || definition.Settings["Stereotype"] != Constants.ProductName)
                yield break;
        }
    }
}