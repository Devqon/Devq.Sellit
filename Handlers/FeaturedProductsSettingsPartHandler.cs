using System;
using Devq.Sellit.Models;
using Devq.Sellit.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;

namespace Devq.Sellit.Handlers
{
    public class FeaturedProductsSettingsPartHandler : ContentHandler {
        private readonly IFeaturedProductService _featuredProductService;

        public Localizer T { get; set; }

        public FeaturedProductsSettingsPartHandler(IFeaturedProductService featuredProductService)
        {
            _featuredProductService = featuredProductService;
            T = NullLocalizer.Instance;

            Filters.Add(new ActivatingFilter<FeaturedProductsSettingsPart>("Site"));
            Filters.Add(new TemplateFilterForPart<FeaturedProductsSettingsPart>("FeaturedProductsSettings", "Parts.FeaturedProducts.SiteSettings", "Sellit"));
            OnUpdated<FeaturedProductsSettingsPart>(ScheduleNewTask);
        }

        private void ScheduleNewTask(UpdateContentContext context, FeaturedProductsSettingsPart part) {
            if (part.TimeLimit.HasValue) {
                if (part.TimeLimit < DateTime.UtcNow) {
                    // Schedule for tomorrow
                    var forDate = DateTime.UtcNow;
                    if (part.TimeLimit.Value.TimeOfDay > forDate.TimeOfDay) {
                        forDate = forDate.AddDays(1);
                    }
                    part.TimeLimit = _featuredProductService.BuildTimeLimit(forDate, part.TimeLimit.Value);
                }
                _featuredProductService.ScheduleNextTask(part.TimeLimit.Value);
            }
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
                return;

            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("Sellit")));
        }
    }
}