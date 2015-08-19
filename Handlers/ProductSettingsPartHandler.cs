using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;

namespace Devq.Sellit.Handlers
{
    public class ProductSettingsPartHandler : ContentHandler
    {
        public Localizer T { get; set; }

        public ProductSettingsPartHandler() {
            T = NullLocalizer.Instance;

            Filters.Add(new ActivatingFilter<ProductSettingsPart>("Site"));
            Filters.Add(new TemplateFilterForPart<ProductSettingsPart>("ProductSettings", "Parts.Product.SiteSettings", "Products"));
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
                return;

            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Products")));
        }
    }
}