using Orchard.ContentManagement;

namespace Devq.Sellit.Models
{
    public class ProductExtension<T> : ContentPart<T>
    {
        public ProductPart ProductPart {
            get { return this.As<ProductPart>(); }
        }
    }
}