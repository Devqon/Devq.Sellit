using Orchard.ContentManagement;
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
            var knowledgeTaxonomy = _taxonomyService.GetTaxonomyByName(Constants.CategoryTaxonomyName);
            if (knowledgeTaxonomy == null)
            {
                knowledgeTaxonomy = _contentManager.New("Taxonomy").As<TaxonomyPart>();
                knowledgeTaxonomy.Name = Constants.CategoryTaxonomyName;
                knowledgeTaxonomy.ContentItem.As<TitlePart>().Title = Constants.CategoryTaxonomyName;
                _taxonomyService.CreateTermContentType(knowledgeTaxonomy);
                _contentManager.Create(knowledgeTaxonomy);
                _contentManager.Publish(knowledgeTaxonomy.ContentItem);
            }
        }
    }
}