using Devq.Sellit.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Devq.Sellit.Handlers
{
    public class ProductPartHandler : ContentHandler {

        public ProductPartHandler(IRepository<ProductPartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
            OnCreated<ProductPart>(SetCategory);
        }

        private void SetCategory(CreateContentContext ctx, ProductPart part) {
            part.Category = ctx.ContentItem.ContentType;
        }
    }
}