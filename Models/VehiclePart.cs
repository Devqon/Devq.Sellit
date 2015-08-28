using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace Devq.Sellit.Models
{
    public class VehiclePart : ContentPart<VehiclePartRecord>
    {
        [Required]
        public int Kilometres {
            get { return Retrieve(x => x.Kilometres); }
            set { Store(x => x.Kilometres, value); }
        }

        [Required]
        public int YearOfManufacture
        {
            get { return Retrieve(x => x.YearOfManufacture); }
            set { Store(x => x.YearOfManufacture, value); }
        }

        [Required]
        public string LicensePlate
        {
            get { return Retrieve(x => x.LicensePlate); }
            set { Store(x => x.LicensePlate, value); }
        } 
    }

    public class VehiclePartRecord : ContentPartRecord {

        public virtual int Kilometres { get; set; }
        public virtual int YearOfManufacture { get; set; }
        public virtual string LicensePlate { get; set; }
    }
}