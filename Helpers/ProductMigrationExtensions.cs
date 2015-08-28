using System.Collections.Generic;
using Devq.Bids.Models;
using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;

namespace Devq.Sellit.Helpers
{
    public static class ProductMigrationExtensions
    {
        /// <summary>
        /// Build the type with common product parts
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContentTypeDefinitionBuilder ProductExtension(this ContentTypeDefinitionBuilder builder) {

            builder

                // Options
                .Listable()

                // Product stereotype
                .WithSetting("Stereotype", Constants.ProductName)

                // Product
                .WithPart(typeof (ProductPart).Name)

                // Bids
                .WithPart(typeof (BidsPart).Name)

                // Common
                .WithPart("TitlePart")
                .WithPart("BodyPart")
                .WithPart("CommonPart")

                // Autoroute
                .WithPart("AutoroutePart", part => part
                    .WithSetting("AutorouteSettings.AllowCustomPattern", "false")
                    .WithSetting("AutorouteSettings.AutomaticAdjustmentOnEdit", "true")
                    .WithSetting("AutorouteSettings.PatternDefinitions",
                        // products/type/title
                        @"[{Name:'Title', Pattern:'products/{Content.ContentType}/{Content.Slug}', Description:'Title'}]")
                    .WithSetting("AutorouteSettings.DefaultPatternIndex", "0"))

                // Tags
                .WithPart("TagsPart");

            return builder;
        }

        /// <summary>
        /// Build the type with common product parts and add the specified part
        /// </summary>
        /// <typeparam name="TPart"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ContentTypeDefinitionBuilder ProductExtension<TPart>(this ContentTypeDefinitionBuilder builder) where TPart : ContentPart {

            return builder.ProductExtension(new List<string> {typeof (TPart).Name});
        }

        /// <summary>
        /// Build the type with common product parts and add an extra part
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="extraPart"></param>
        /// <returns></returns>
        public static ContentTypeDefinitionBuilder ProductExtension(this ContentTypeDefinitionBuilder builder, string extraPart) {

            return builder.ProductExtension(new List<string> {extraPart});
        }

        /// <summary>
        /// Build the type with common product parts and add the extra parts
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="extraParts"></param>
        /// <returns></returns>
        public static ContentTypeDefinitionBuilder ProductExtension(this ContentTypeDefinitionBuilder builder, List<string> extraParts) {

            builder

                // All common extensions
                .ProductExtension();

            // Add extra parts
            foreach (var extraPart in extraParts) {
                builder.WithPart(extraPart);
            }

            return builder;
        }

        /// <summary>
        /// Create a product content type with the common product parts and add the extra parts
        /// </summary>
        /// <param name="cdm"></param>
        /// <param name="type"></param>
        /// <param name="extraParts"></param>
        public static void CreateProductType(this IContentDefinitionManager cdm, string type, List<string> extraParts = null)
        {
            // Create type and attach parts
            cdm.AlterTypeDefinition(type,
                builder => builder
                    .ProductExtension(extraParts ?? new List<string>()));
        }

        /// <summary>
        /// Create a batch of product content types with the common product parts and add the extra parts
        /// </summary>
        /// <param name="cdm"></param>
        /// <param name="types"></param>
        /// <param name="extraParts"></param>
        public static void CreateProductTypes(this IContentDefinitionManager cdm, string[] types, List<string> extraParts = null) {

            foreach (var type in types) {
                cdm.CreateProductType(type, extraParts);
            }
        }

        /// <summary>
        /// Create a batch of product content types with the common product parts and add the extra part
        /// </summary>
        /// <param name="cdm"></param>
        /// <param name="type"></param>
        public static void CreateProductType<TExtraPart>(this IContentDefinitionManager cdm, string type) where TExtraPart : ContentPart
        {
            cdm.CreateProductType(type, new List<string> { typeof(TExtraPart).Name });
        }

        /// <summary>
        /// Create a batch of product content types with the common product parts and add the extra part
        /// </summary>
        /// <param name="cdm"></param>
        /// <param name="types"></param>
        public static void CreateProductTypes<TExtraPart>(this IContentDefinitionManager cdm, string[] types) where TExtraPart : ContentPart
        {
            foreach (var type in types)
            {
                cdm.CreateProductType<TExtraPart>(type);
            }
        }
    }
}