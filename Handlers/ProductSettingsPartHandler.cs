using System;
using System.Linq;
using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Core.Common.Models;
using Orchard.Localization;
using Orchard.Tasks.Scheduling;

namespace Devq.Sellit.Handlers
{
    public class ProductSettingsPartHandler : ContentHandler {
        private readonly IScheduledTaskManager _scheduledTaskManager;
        private readonly IContentManager _contentManager;

        public Localizer T { get; set; }

        public ProductSettingsPartHandler(IScheduledTaskManager scheduledTaskManager, IContentManager contentManager) {
            _scheduledTaskManager = scheduledTaskManager;
            _contentManager = contentManager;

            T = NullLocalizer.Instance;

            Filters.Add(new ActivatingFilter<ProductSettingsPart>("Site"));
            Filters.Add(new TemplateFilterForPart<ProductSettingsPart>("ProductSettings", "Parts.Product.SiteSettings", "Sellit"));

            OnUpdated<ProductSettingsPart>(RescheduleExistingProducts);
        }

        private void RescheduleExistingProducts(UpdateContentContext updateContentContext, ProductSettingsPart part) {

            var existingTasks = _scheduledTaskManager
                .GetTasks(Constants.UnpublishTaskName)
                .ToList();

            _scheduledTaskManager.DeleteTasks(null, t => t.TaskType == Constants.UnpublishTaskName);

            if (part.HideProductDelay > 0) {
                foreach (var task in existingTasks) {
                    if (task.ContentItem != null) {
                        var published = task.ContentItem.As<CommonPart>();
                        if (published != null && published.PublishedUtc.HasValue) {
                            _scheduledTaskManager.CreateTask(Constants.UnpublishTaskName, published.PublishedUtc.Value.AddDays(part.HideProductDelay), task.ContentItem);
                        }
                    }
                }
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