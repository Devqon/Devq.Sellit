using Devq.Sellit.Models;
using Orchard.Data.Migration;
using Orchard.Environment.Extensions;

namespace Devq.Sellit
{
    [OrchardFeature("Devq.ProductExtensions")]
    public class ProductExtensionsMigrations : DataMigrationImpl {

        public int Create() {

            SchemaBuilder.CreateTable(typeof (VehiclePartRecord).Name,
                table => table
                    .ContentPartRecord()

                    .Column<int>("Kilometres")
                    .Column<int>("YearOfManufacture")
                    .Column<string>("LicensePlate"));

            return 1;
        }
    }
}