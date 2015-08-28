using System.Collections.Generic;
using System.Linq;
using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.Core.Common.Models;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;

namespace Devq.Sellit.Services {
    public class ProductService : IProductService {

        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ITaxonomyService _taxonomyService;

        public ProductService(IContentManager contentManager, 
            IContentDefinitionManager contentDefinitionManager, 
            ITaxonomyService taxonomyService) {
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _taxonomyService = taxonomyService;
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
                .Where(t => t.Parts.Any(p => p.PartDefinition.Name.Equals(typeof (ProductPart).Name)));
        }

        public IEnumerable<TermPart> GetTermCategories() {
            var taxonomy = _taxonomyService.GetTaxonomyByName(Constants.CategoryTaxonomyName);
            var terms = _taxonomyService.GetTerms(taxonomy.Id);

            return terms;
        } 

        public void CreateCategories(string[] categories, string parent = null, bool selectable = false) {
            var taxonomy = _taxonomyService.GetTaxonomyByName(Constants.CategoryTaxonomyName);
            foreach (var cat in categories) {
                CreateCategory(taxonomy, cat, parent, selectable);
            }
        }

        public void CreateCategory(TaxonomyPart taxonomy, string category, string parent = null, bool selectable = false) {
            var term = _taxonomyService.GetTermByName(taxonomy.Id, category);
            if (term == null)
            {
                if (parent == null)
                {
                    term = _taxonomyService.NewTerm(taxonomy);
                }
                else
                {
                    var parentTerm = _taxonomyService.GetTermByName(taxonomy.Id, parent);
                    // If parent doesnt exist, create it
                    if (parentTerm == null)
                    {
                        CreateCategory(taxonomy, parent);
                    }
                    term = _taxonomyService.NewTerm(taxonomy, parentTerm);
                }
                term.Name = category;
                term.Selectable = selectable;
                _contentManager.Create(term);
            }
        }
    }
}