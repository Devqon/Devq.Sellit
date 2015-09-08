using System;
using System.Collections.Generic;
using System.Linq;
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

            return GetContainables(taxonomy.Record.ContentItemRecord).List();
        }

        public IEnumerable<TermPart> GetDirectChildren(int termId) {
            var term = _taxonomyService.GetTerm(termId);

            return GetDirectChildren(term);
        }

        public IEnumerable<TermPart> GetDirectChildren(TermPart term) {
            var directChildren = GetContainables(term.Record.ContentItemRecord);

            return directChildren.List();
        }

        public IContentQuery<TermsPart, TermsPartRecord> GetDirectContentItemsQuery(TermPart term, string fieldName = null)
        {
            var query = _contentManager
                .Query<TermsPart, TermsPartRecord>();

            if (String.IsNullOrWhiteSpace(fieldName))
            {
                query = query.Where(
                    tpr => tpr.Terms.Any(tr =>
                        tr.TermRecord.Id == term.Id));
            }
            else
            {
                query = query.Where(
                    tpr => tpr.Terms.Any(tr =>
                        tr.Field == fieldName
                         && (tr.TermRecord.Id == term.Id)));
            }

            return query;
        }

        public long GetDirectContentItemsCount(TermPart term, string fieldName = null)
        {
            return GetDirectContentItemsQuery(term, fieldName).Count();
        }

        public IEnumerable<IContent> GetDirectContentItems(TermPart term, int skip = 0, int count = 0, string fieldName = null)
        {
            return GetDirectContentItemsQuery(term, fieldName)
                .Join<CommonPartRecord>()
                .OrderByDescending(x => x.CreatedUtc)
                .Slice(skip, count);
        }

        private IContentQuery<TermPart> GetContainables(ContentItemRecord record) {
            return _contentManager
                .Query<TermPart>()
                .Join<CommonPartRecord>()
                .Where(cr => cr.Container == record);
        } 
    }
}