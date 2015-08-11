using Devq.Sellit.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Devq.Sellit.Handlers
{
    public class PriceChoicePartHandler : ContentHandler
    {
        public PriceChoicePartHandler(IRepository<PriceChoicePartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}