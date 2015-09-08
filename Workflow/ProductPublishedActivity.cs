using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.Workflows.Activities;

namespace Devq.Sellit.Workflow
{
    [OrchardFeature("Devq.ProductWorkflows")]
    public class ProductPublishedActivity : ContentActivity
    {
        public override string Name {
            get { return "ProductPublished"; }
        }

        public override LocalizedString Description {
            get { return T("Product is published"); }
        }
    }
}