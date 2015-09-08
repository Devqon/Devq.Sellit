using System.Collections.Generic;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Taxonomies.Models;

namespace Devq.Sellit.Services
{
    public interface ICategoryService : IDependency {
        void EnsureCategoryTaxonomy();
        IEnumerable<TermPart> GetDirectChildren(TermPart term);
        IEnumerable<TermPart> GetTopLevelTerms(string taxonomyName);
        IEnumerable<TermPart> GetDirectChildren(int termId);
        IContentQuery<TermsPart, TermsPartRecord> GetDirectContentItemsQuery(TermPart term, string fieldName = null);
        long GetDirectContentItemsCount(TermPart term, string fieldName = null);
        IEnumerable<IContent> GetDirectContentItems(TermPart term, int skip = 0, int count = 0, string fieldName = null);
    }
}