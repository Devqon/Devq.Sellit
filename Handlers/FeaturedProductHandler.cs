using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment.Extensions;

namespace Devq.Sellit.Handlers
{
    [OrchardFeature("Devq.FeaturedProducts")]
    public class FeaturedProductHandler : ContentHandler {
        private readonly IContentManager _contentManager;

        public FeaturedProductHandler(
            IRepository<FeaturedProductPartRecord> repository, 
            IContentManager contentManager) {
            _contentManager = contentManager;

            Filters.Add(StorageFilter.For(repository));

            OnLoaded<FeaturedProductPart>(LazyLoadHandlers);
            OnInitializing<FeaturedProductPart>(PropertySetHandlers);
        }

        private void LazyLoadHandlers(LoadContentContext context, FeaturedProductPart part) {
            part
                ._productField
                .Loader(prt => part.Record.Product == null ? null : _contentManager.Get(part.Record.Product.Id));
        }

        static void PropertySetHandlers(InitializingContentContext context, FeaturedProductPart part)
        {
            // add handlers that will update records when part properties are set
            part._productField.Setter(product =>
            {
                part.Record.Product = product == null
                    ? null
                    : product.ContentItem.Record;
                return product;
            });

            // Force call to setter if we had already set a value
            if (part._productField.Value != null)
                part._productField.Value = part._productField.Value;
        }
    }
}