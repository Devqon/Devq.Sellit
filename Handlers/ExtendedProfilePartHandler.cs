using Devq.Sellit.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Devq.Sellit.Handlers
{
    public class ExtendedProfilePartHandler : ContentHandler
    {
        public ExtendedProfilePartHandler(IRepository<ExtendedProfilePartRecord> repository) {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}