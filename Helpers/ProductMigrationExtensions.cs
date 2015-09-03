using System.Collections.Generic;
using Devq.Bids.Models;
using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Utility.Extensions;

namespace Devq.Sellit.Helpers
{
    public static class ProductMigrationExtensions
    {
        /// <summary>
        /// Build the type with common product parts
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public static ContentTypeDefinitionBuilder ProductExtension(this ContentTypeDefinitionBuilder builder, string category = null) {

            builder

                // Options
                .Listable()

                // Product stereotype
                .WithSetting("Stereotype", Constants.ProductName)

                // Product
                .WithPart(typeof (ProductPart).Name, part => {
                    if (!string.IsNullOrEmpty(category)) {
                        part.WithSetting("ProductPartSettings.CategoryId", category);
                    }
                })

                // Bids
                .WithPart(typeof (BidsPart).Name)

                // Common
                .WithPart("TitlePart")
                .WithPart("BodyPart")
                .WithPart("CommonPart", part => part
                    // Do not edit owner
                    .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "false"))

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
        /// <param name="category"></param>
        /// <returns></returns>
        public static ContentTypeDefinitionBuilder ProductExtension(this ContentTypeDefinitionBuilder builder, string extraPart, string category = null) {

            return builder.ProductExtension(new List<string> {extraPart}, category);
        }

        /// <summary>
        /// Build the type with common product parts and add the extra parts
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="extraParts"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public static ContentTypeDefinitionBuilder ProductExtension(this ContentTypeDefinitionBuilder builder, List<string> extraParts, string category = null) {

            builder

                // All common extensions
                .ProductExtension(category);

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
        /// <param name="category"></param>
        public static void CreateProductType(this IContentDefinitionManager cdm, string type, List<string> extraParts = null, string category = null)
        {
            // Create type and attach parts
            cdm.AlterTypeDefinition(type.ToSafeName(),
                builder => builder
                    .ProductExtension(extraParts ?? new List<string>(), category)
                    .DisplayedAs(type));
        }

        /// <summary>
        /// Create a batch of product content types with the common product parts and add the extra parts
        /// </summary>
        /// <param name="cdm"></param>
        /// <param name="types"></param>
        /// <param name="extraParts"></param>
        /// <param name="category"></param>
        public static void CreateProductTypes(this IContentDefinitionManager cdm, string[] types, List<string> extraParts = null, string category = null) {

            foreach (var type in types) {
                cdm.CreateProductType(type, extraParts, category);
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