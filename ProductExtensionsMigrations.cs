using Devq.Sellit.Helpers;
using Devq.Sellit.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace Devq.Sellit
{
    public class ProductExtensionsMigrations : DataMigrationImpl
    {
        public int Create() {

            SchemaBuilder.CreateTable(typeof (VehiclePartRecord).Name,
                table => table
                    .ContentPartRecord()

                    .Column<int>("Kilometres")
                    .Column<int>("YearOfManufacture")
                    .Column<string>("LicensePlate"));

            return 1;
        }

        public int UpdateFrom1() {
            
            // Car
            ContentDefinitionManager.AlterTypeDefinition("Car",
                type => type
                    .ProductExtension<VehiclePart>());

            return 2;
        }
    }
}