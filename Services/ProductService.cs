using System;
using System.Collections.Generic;
using System.Linq;
using Devq.Sellit.Helpers;
using Devq.Sellit.Models;
using Devq.Sellit.Settings;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.Core.Common.Models;
using Orchard.Security;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;

namespace Devq.Sellit.Services {
    public class ProductService : IProductService {

        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ITaxonomyService _taxonomyService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IOrchardServices _orchardServices;

        public ProductService(IContentManager contentManager, 
            IContentDefinitionManager contentDefinitionManager, 
            ITaxonomyService taxonomyService, IAuthorizationService authorizationService, IOrchardServices orchardServices) {
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _taxonomyService = taxonomyService;
            _authorizationService = authorizationService;
            _orchardServices = orchardServices;
        }

        public IContentQuery<ProductPart> GetProductsQuery() {
            return _contentManager
                .Query<ProductPart, ProductPartRecord>();
        }

        public IEnumerable<ContentTypeDefinition> GetCategories() {
            return _contentDefinitionManager
                .ListTypeDefinitions()
                .Where(t => t.Parts.Any(p => p.PartDefinition.Name.Equals(typeof (ProductPart).Name)));
        }

        public ContentTypeDefinition GetTypeByCategory(string category) {
            var products = GetCategories();

            return products.FirstOrDefault(prod => {
                var prodPart = prod.Parts.FirstOrDefault(p => p.PartDefinition.Name.Equals(typeof (ProductPart).Name));
                if (prodPart == null)
                    return false;

                var settings = prodPart.Settings.GetModel<ProductPartSettings>();
                return settings.CategoryId == category;
            });
        }

        public IEnumerable<Tuple<string, string>> GetProductTypes()
        {
            return _contentManager.GetContentTypeDefinitions()
                .Where(contentTypeDefinition => contentTypeDefinition.Settings.ContainsKey("Stereotype") && contentTypeDefinition.Settings["Stereotype"] == Constants.ProductName)
                .Select(contentTypeDefinition =>
                    Tuple.Create(
                        contentTypeDefinition.Name,
                        contentTypeDefinition.Settings.ContainsKey("Description") ? contentTypeDefinition.Settings["Description"] : contentTypeDefinition.DisplayName));
        }

        public IEnumerable<TermPart> GetTermCategories() {
            var taxonomy = _taxonomyService.GetTaxonomyByName(Constants.CategoryTaxonomyName);
            var terms = _taxonomyService.GetTerms(taxonomy.Id);

            return terms;
        }

        public bool CreateProductType(string name) {
            var definition = _contentDefinitionManager.GetTypeDefinition(name);
            if (definition != null) {
                return false;
            }

            _contentDefinitionManager.CreateProductType(name);
            return true;
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