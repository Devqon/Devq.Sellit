using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;

namespace Devq.Sellit.Models
{
    public class ProductSettingsPart : ContentPart
    {
        [Range(0, int.MaxValue, ErrorMessage = "Must be a positive number")]
        public int HideProductDelay {
            get { return this.Retrieve(r => r.HideProductDelay); }
            set { this.Store(r => r.HideProductDelay, value); }
        }

        public int ProductSelectSize {
            get { return this.Retrieve(r => r.ProductSelectSize); }
            set { this.Store(r => r.ProductSelectSize, value); }
        }
    }
}