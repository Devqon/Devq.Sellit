using System.Collections.Generic;
using Devq.Sellit.Models;
using Devq.Sellit.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.ViewModels;

namespace Devq.Sellit.Settings
{
    public class ProductPartSettingsEvents : ContentDefinitionEditorEventsBase {

        private readonly IProductService _productService;

        public ProductPartSettingsEvents(IProductService productService) {
            _productService = productService;
        }

        public override IEnumerable<TemplateViewModel> TypePartEditor(ContentTypePartDefinition definition) {
            if (definition.PartDefinition.Name != typeof (ProductPart).Name)
                yield break;

            var settings = definition.Settings.GetModel<ProductPartSettings>();

            var categories = _productService.GetTermCategories();
            settings.Categories = categories;
            yield return DefinitionTemplate(settings);
        }

        public override IEnumerable<TemplateViewModel> TypePartEditorUpdate(ContentTypePartDefinitionBuilder builder, IUpdateModel updateModel) {

            if (builder.Name != typeof (ProductPart).Name)
                yield break;

            var settings = new ProductPartSettings();

            updateModel.TryUpdateModel(settings, typeof (ProductPartSettings).Name, null, null);
            builder.WithSetting("ProductPartSettings.PostCosts", settings.PostCosts.ToString());
            builder.WithSetting("ProductPartSettings.CategoryId", settings.CategoryId);

            yield return DefinitionTemplate(settings);
        }
    }
}