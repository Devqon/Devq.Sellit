using System;
using Devq.Sellit.Models;
using Devq.Sellit.Services;
using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace Devq.Sellit
{
    public class Migrations : DataMigrationImpl {
        private readonly ICategoryService _categoryService;

        public Migrations(ICategoryService categoryService) {
            _categoryService = categoryService;
        }

        public int Create() {

            // Ensure the category taxonomy is there
            _categoryService.EnsureCategoryTaxonomy();

            // Product part
            ContentDefinitionManager.AlterPartDefinition(typeof (ProductPart).Name, part => part

                .Attachable(false)

                // Category taxonomy field
                .WithField("Category", field => field
                    .OfType("TaxonomyField")
                    .WithSetting("TaxonomyFieldSettings.Autocomplete", "true")
                    .WithSetting("TaxonomyFieldSettings.Taxonomy", Constants.CategoryTaxonomyName))

                // Url field
                .WithField("Url", field => field
                    .OfType("InputField")
                    .WithSetting("InputFieldSettings.Required", "false")
                    .WithSetting("InputFieldSettings.Type", "Url")
                    .WithSetting("InputFieldSettings.Hint", "Video or website url")));

            // Product type
            ContentDefinitionManager.AlterTypeDefinition("Product", type => type

                .Creatable()
                .Listable()

                // Common parts
                .WithPart("TitlePart")
                .WithPart("BodyPart")
                .WithPart("CommonPart")
                .WithPart("AutoroutePart")
                .WithPart("TagsPart")

                // Product part
                .WithPart(typeof (ProductPart).Name));

            return 1;
        }

        public int UpdateFrom1() {
            
            // Level terms widget
            ContentDefinitionManager.AlterTypeDefinition("LevelTermsWidget", type => type
                
                .Creatable()

                .WithPart(typeof(LevelTermsWidgetPart).Name)
                .WithPart("WidgetPart")
                .WithPart("CommonPart")
                .WithSetting("Stereotype", "Widget"));

            return 2;
        }

        public int UpdateFrom2() {

            SchemaBuilder.CreateTable(typeof (LevelTermsWidgetPartRecord).Name, table => table

                .ContentPartRecord()

                .Column<string>("ForTaxonomy"));

            return 3;
        }
    }
}