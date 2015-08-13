using System.Collections.Generic;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;

namespace Devq.Sellit.Services {
    public class CategoryService : ICategoryService {

        private readonly ITaxonomyService _taxonomyService;
        private readonly IContentManager _contentManager;

        public CategoryService(ITaxonomyService taxonomyService, IContentManager contentManager) {
            _taxonomyService = taxonomyService;
            _contentManager = contentManager;
        }

        public void EnsureCategoryTaxonomy()
        {
            var categoryTaxonomy = _taxonomyService.GetTaxonomyByName(Constants.CategoryTaxonomyName);
            if (categoryTaxonomy == null)
            {
                // Create the taxonomy
                categoryTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
                categoryTaxonomy.Name = Constants.CategoryTaxonomyName;
                categoryTaxonomy.ContentItem.As<TitlePart>().Title = Constants.CategoryTaxonomyName;
                // Create the term type
                _taxonomyService.CreateTermContentType(categoryTaxonomy);
                _contentManager.Create(categoryTaxonomy);
                _contentManager.Publish(categoryTaxonomy.ContentItem);
            }
        }

        public IEnumerable<TermPart> GetTopLevelTerms(string taxonomyName) {
            var taxonomy = _taxonomyService.GetTaxonomyByName(taxonomyName);

            return GetChildren(taxonomy.Record.ContentItemRecord).List();
        } 

        public IEnumerable<TermPart> GetDirectChildren(TermPart term) {
            var directChildren = GetChildren(term.Record.ContentItemRecord);

            return directChildren.List();
        }

        private IContentQuery<TermPart> GetChildren(ContentItemRecord record) {
            return _contentManager
                .Query<TermPart>()
                .Join<CommonPartRecord>()
                .Where(cr => cr.Container == record);
        } 
    }
}