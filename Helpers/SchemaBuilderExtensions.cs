using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;

namespace Devq.Sellit.Helpers
{
    public static class SchemaBuilderExtensions
    {
        /// <summary>
        /// Build the type with product parts that all products have
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContentTypeDefinitionBuilder ProductExtension(this ContentTypeDefinitionBuilder builder) {

            builder

                // Options
                .Listable()

                // Product stereotype
                .WithSetting("Stereotype", Constants.ProductName)

                // Product part
                .WithPart(typeof (ProductPart).Name)

                // Common parts
                .WithPart("TitlePart")
                .WithPart("BodyPart")
                .WithPart("CommonPart")
                .WithPart("AutoroutePart")
                .WithPart("TagsPart");

            return builder;
        }

        /// <summary>
        /// Build the type with product parts that all products have and add the specific part
        /// </summary>
        /// <typeparam name="TPart"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContentTypeDefinitionBuilder ProductExtension<TPart>(this ContentTypeDefinitionBuilder builder) where TPart : ContentPart {
            
            builder

                // All product parts
                .ProductExtension()

                // Extension part
                .WithPart(typeof(TPart).Name);

            return builder;
        }
    }
}