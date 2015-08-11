using Devq.Sellit.Models;
using Devq.Sellit.Services;
using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace Devq.Sellit
{
    public class Migrations : DataMigrationImpl {
        private readonly ICategoryService _categoryService;

        public Migrations(ICategoryService categoryService) {
            _categoryService = categoryService;
        }

        public int Create() {

            _categoryService.EnsureCategoryTaxonomy();

            ContentDefinitionManager.AlterPartDefinition(typeof(ProductPart).Name, part => part
                
                .WithField("Category", field => field
                    .OfType("TaxonomyField")
                    .WithSetting("TaxonomyFieldSettings.Autocomplete", "true")
                    .WithSetting("TaxonomyFieldSettings.Taxonomy", Constants.CategoryTaxonomyName)));

            ContentDefinitionManager.AlterTypeDefinition("Product", type => type

                .WithPart("TitlePart")
                .WithPart("BodyPart")
                .WithPart("CommonPart")

                .WithPart(typeof (ProductPart).Name));

            return 1;
        }
    }
}