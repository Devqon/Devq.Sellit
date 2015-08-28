using System.Collections.Generic;
using Devq.Sellit.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData.Models;

namespace Devq.Sellit.Services
{
    public interface IProductService : IDependency {
        IContentQuery<ProductPart> GetProducts();
        IEnumerable<ContentTypeDefinition> GetCategories();
    }
}
