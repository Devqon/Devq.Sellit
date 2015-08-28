using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.ContentManagement.Records;

namespace Devq.Sellit.Models
{
    public class ExtendedProfilePart : ContentPart<ExtendedProfilePartRecord>, ITitleAspect
    {
        public string FirstName {
            get { return Retrieve(x => x.FirstName); }
            set { Store(x => x.FirstName, value); }
        }

        public string LastName
        {
            get { return Retrieve(x => x.LastName); }
            set { Store(x => x.LastName, value); }
        }

        public string Insertion
        {
            get { return Retrieve(x => x.Insertion); }
            set { Store(x => x.Insertion, value); }
        }

        public string FullName
        {
            get { return string.Format("{0}{1} {2}", FirstName, string.IsNullOrEmpty(Insertion) ? "" : " " + Insertion, LastName); }
        }

        public string Title {
            get { return FullName; }
        }
    }

    public class ExtendedProfilePartRecord : ContentPartRecord {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Insertion { get; set; }
    }
}