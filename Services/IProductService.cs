using Orchard;
using Orchard.ContentManagement;

namespace Devq.Sellit.Services
{
    public interface IProductService : IDependency {
        ContentPart GetNewExtensionPart(string category);
    }
}