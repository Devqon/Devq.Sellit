using System;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.ContentManagement.Records;
using Orchard.ContentManagement.Utilities;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using Orchard.Security;

namespace Devq.Sellit.Models
{
    public class FeaturedProductPart : ContentPart<FeaturedProductPartRecord>, ITitleAspect {
        internal LazyField<IContent> _productField = new LazyField<IContent>(); 

        public IContent Product {
            get { return _productField.Value; }
            set { _productField.Value = value; }
        }

        public DateTime? Date {
            get { return Retrieve(r => r.Date); }
            set { Store(r => r.Date, value); }
        }

        public int Number {
            get { return Retrieve(r => r.Number); }
            set { Store(r => r.Number, value); }
        }

        public IUser User {
            get { return Product == null ? null : Product.As<CommonPart>().Owner; }
        }

        public string Title {
            get {
                // No product picked yet
                if (Product == null) {
                    return string.Format("{0} - {1}", Number, Date.Value.ToString("dd-MM-yyyy"));
                }

                // Else use the product's title
                return Product.As<TitlePart>().Title;
            }
        }
    }

    public class FeaturedProductPartRecord : ContentPartRecord {

        public virtual ContentItemRecord Product { get; set; }
        public virtual DateTime? Date { get; set; }
        public virtual int Number { get; set; }
    }
}