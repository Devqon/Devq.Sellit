using Orchard.DisplayManagement.Descriptors;
using Orchard.Taxonomies.Fields;

namespace Devq.Sellit
{
    public class EditorFieldShapeProvider : IShapeTableProvider
    {
        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("EditorTemplate")
                .OnDisplaying(displaying => {
                    var shape = displaying.Shape;

                    if (shape.ContentItem.ContentType == Constants.ProductName && shape.ContentField is TaxonomyField) {
                        shape.TemplateName = "Fields/TaxonomyField-Product";
                    }
                });
        }
    }
}