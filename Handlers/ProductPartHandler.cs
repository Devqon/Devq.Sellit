using System.Collections.Generic;
using Devq.Sellit.Models;
using Devq.Sellit.Settings;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;

namespace Devq.Sellit.Handlers
{
    public class ProductPartHandler : ContentHandler {

        private readonly ITaxonomyService _taxonomyService;

        public ProductPartHandler(IRepository<ProductPartRecord> repository, 
            ITaxonomyService taxonomyService) {

            _taxonomyService = taxonomyService;
            Filters.Add(StorageFilter.For(repository));
            OnCreated<ProductPart>(SetCategory);
        }

        private void SetCategory(CreateContentContext ctx, ProductPart part) {

            var settings = part.Settings.GetModel<ProductPartSettings>();

            var category = settings.CategoryId;
            int categoryId;
            if (category == "0" || !int.TryParse(category, out categoryId))
                return;

            var term = _taxonomyService.GetTerm(categoryId);

            // Store category in part
            part.Category = term.Name;

            // As well as in the termspart, to make use of the taxonomies
            _taxonomyService.UpdateTerms(part.ContentItem, new List<TermPart>{term}, Constants.CategoryTaxonomyName);
        }
    }
}