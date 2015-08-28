using System.Collections.Generic;
using Devq.Sellit.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.Taxonomies.Models;

namespace Devq.Sellit.Services
{
    public interface IProductService : IDependency {
        IContentQuery<ProductPart> GetProducts();
        IEnumerable<ContentTypeDefinition> GetCategories();
        void CreateCategories(string[] categories, string parent = null, bool selectable = false);
        void CreateCategory(TaxonomyPart taxonomy, string category, string parent = null, bool selectable = false);
        IEnumerable<TermPart> GetTermCategories();
    }
}
