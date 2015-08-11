using Orchard;

namespace Devq.Sellit.Services
{
    public interface ICategoryService : IDependency {
        void EnsureCategoryTaxonomy();
    }
}