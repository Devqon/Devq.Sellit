using Orchard.ContentManagement;
using Orchard.DisplayManagement.Descriptors;

namespace Devq.Sellit {
    public class ControlWrapper : IShapeTableProvider {
        public void Discover(ShapeTableBuilder builder) {
            builder.Describe("Product")
                .OnCreated(created =>
                {
                    var content = created.Shape;
                    content.Child.Add(created.New.PlaceChildContent(Source: content));
                })
                .OnDisplaying(displaying => {
                    if (displaying.ShapeMetadata.DisplayType == "Detail") {
                        displaying.ShapeMetadata.Wrappers.Add("Product_EditWrapper");
                    }
                    ContentItem contentItem = displaying.Shape.ContentItem;
                    if (contentItem != null)
                    {
                        // Alternates in order of specificity. 
                        // Display type > content type > specific content > display type for a content type > display type for specific content
                        // BasicShapeTemplateHarvester.Adjust will then adjust the template name

                        // Product__[DisplayType] e.g. Product-Summary
                        displaying.ShapeMetadata.Alternates.Add("Product_" + EncodeAlternateElement(displaying.ShapeMetadata.DisplayType));

                        // Product__[ContentType] e.g. Product-BlogPost,
                        displaying.ShapeMetadata.Alternates.Add("Product__" + EncodeAlternateElement(contentItem.ContentType));

                        // Product_[DisplayType]__[ContentType] e.g. Product-BlogPost.Summary
                        displaying.ShapeMetadata.Alternates.Add("Product_" + displaying.ShapeMetadata.DisplayType + "__" + EncodeAlternateElement(contentItem.ContentType));
                    }
                });
        }

        /// <summary>
        /// Encodes dashed and dots so that they don't conflict in filenames 
        /// </summary>
        /// <param name="alternateElement"></param>
        /// <returns></returns>
        private string EncodeAlternateElement(string alternateElement)
        {
            return alternateElement.Replace("-", "__").Replace(".", "_");
        }
    }
}