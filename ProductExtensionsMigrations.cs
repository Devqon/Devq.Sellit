using Devq.Sellit.Models;
using Orchard.Data.Migration;

namespace Devq.Sellit
{
    public class ProductExtensionsMigrations : DataMigrationImpl
    {
        public int Create() {

            SchemaBuilder.CreateTable(typeof (VehiclePartRecord).Name,
                table => table
                    .ContentPartRecord()

                    .Column<int>("Kilometres"));

            return 1;
        }
    }
}