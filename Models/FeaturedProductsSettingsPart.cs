using System;
using Orchard.ContentManagement;

namespace Devq.Sellit.Models
{
    public class FeaturedProductsSettingsPart : ContentPart
    {
        public int NumberOfFeaturedProducts {
            get { return this.Retrieve(r => r.NumberOfFeaturedProducts); }
            set { this.Store(r => r.NumberOfFeaturedProducts, value); }
        }

        public DateTime? TimeLimit {
            get { return this.Retrieve(r => r.TimeLimit); }
            set { this.Store(r => r.TimeLimit, value); }
        }
    }
}