using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Environment.Extensions;
using Orchard.Workflows.Services;

namespace Devq.Sellit.Handlers
{
    [OrchardFeature("Devq.ProductWorkflows")]
    public class WorkflowProductHandler : ContentHandler
    {
        public WorkflowProductHandler(IWorkflowManager workflowManager) {
            OnPublished<ContentPart>(
                (ctx, part) => {
                    var settings = ctx.ContentItem.TypeDefinition.Settings;
                    var isProduct = settings.ContainsKey("Stereotype") && settings["Stereotype"] == Constants.ProductName;
                    if (!isProduct)
                        return;

                    workflowManager.TriggerEvent("ProductPublished",
                        ctx.ContentItem,
                        () => new Dictionary<string, object>{{"Content", ctx.ContentItem}});
                });
        }
    }
}