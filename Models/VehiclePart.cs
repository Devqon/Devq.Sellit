using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;

namespace Devq.Sellit.Models
{
    public class VehiclePart : ContentPart<VehiclePartRecord> {

        public int Kilometres
        {
            get { return Retrieve(x => x.Kilometres); }
            set { Store(x => x.Kilometres, value); }
        }

        //public int Kilometres {
        //    get { return Record.Kilometres; }
        //    set { Record.Kilometres = value; }
        //}
    }

    public class VehiclePartRecord : ContentPartRecord
    {
        public virtual int Kilometres { get; set; }
    }
}