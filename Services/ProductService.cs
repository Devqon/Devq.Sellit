using System.Collections.Generic;
using System.Linq;
using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.Core.Common.Models;

namespace Devq.Sellit.Services {
    public class ProductService : IProductService {

        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public ProductService(IContentManager contentManager, 
            IContentDefinitionManager contentDefinitionManager) {
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
        }

        public IContentQuery<ProductPart> GetProducts() {
            return _contentManager
                .Query<ProductPart, ProductPartRecord>()
                .ForVersion(VersionOptions.Latest)
                .WithQueryHints(new QueryHints().ExpandParts<CommonPart>());
        }

        public IEnumerable<ContentTypeDefinition> GetCategories() {
            return _contentDefinitionManager
                .ListTypeDefinitions()
                .Where(t => t.Parts.Any(p => p.PartDefinition.Name.Equals(typeof(ProductPart).Name)));
        } 
    }
}