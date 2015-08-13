using System.Collections.Generic;
using Orchard;
using Orchard.Taxonomies.Models;

namespace Devq.Sellit.Services
{
    public interface ICategoryService : IDependency {
        void EnsureCategoryTaxonomy();
        IEnumerable<TermPart> GetDirectChildren(TermPart term);
        IEnumerable<TermPart> GetTopLevelTerms(string taxonomyName);
    }
}