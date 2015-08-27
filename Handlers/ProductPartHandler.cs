using System.Linq;
using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Taxonomies.Models;

namespace Devq.Sellit.Handlers
{
    public class ProductPartHandler : ContentHandler {

        public ProductPartHandler(IRepository<ProductPartRecord> repository) {

            Filters.Add(StorageFilter.For(repository));

            OnUpdated<ProductPart>(SetCategory);
        }

        private void SetCategory(UpdateContentContext ctx, ProductPart part) {
            var termsPart = part.As<TermsPart>();
            if (termsPart == null || !termsPart.TermParts.Any())
                return;

            var term = termsPart.TermParts.First();

            part.Category = term.TermPart.Name;
        }
    }
}