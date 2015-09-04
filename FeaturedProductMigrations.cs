using System;
using Devq.Bids.Models;
using Devq.Sellit.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace Devq.Sellit
{
    [OrchardFeature("Devq.FeaturedProducts")]
    public class FeaturedProductMigrations : DataMigrationImpl
    {
        public int Create() {

            ContentDefinitionManager.AlterTypeDefinition("FeaturedProductsWidget",
                type => type

                    .WithSetting("Stereotype", "Widget")

                    .WithPart(typeof (FeaturedProductsWidget).Name)
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart"));

            return 1;
        }

        public int UpdateFrom1() {

            SchemaBuilder.CreateTable(typeof (FeaturedProductPartRecord).Name,
                table => table
                    .ContentPartRecord()

                    .Column<int>("Product_Id")
                    .Column<DateTime>("Date")
                    .Column<int>("Number"));

            ContentDefinitionManager.AlterTypeDefinition("FeaturedProduct",
                type => type
                    .WithPart(typeof (FeaturedProductPart).Name)
                    .WithPart("CommonPart"));

            return 2;
        }

        public int UpdateFrom2() {

            ContentDefinitionManager.AlterTypeDefinition("FeaturedProduct",
                type => type
                    .WithPart(typeof(BidsPart).Name));

            return 3;
        }
    }
}