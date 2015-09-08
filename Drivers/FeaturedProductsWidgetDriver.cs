using System.Collections.Generic;
using System.Linq;
using Devq.Bids;
using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Environment.Extensions;

namespace Devq.Sellit.Drivers
{
    [OrchardFeature("Devq.FeaturedProducts")]
    public class FeaturedProductsWidgetDriver : ContentPartDriver<FeaturedProductsWidget> {
        private readonly IContentManager _contentManager;
        public FeaturedProductsWidgetDriver(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        protected override DriverResult Display(FeaturedProductsWidget part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_FeaturedProductsWidget", () => {

                var products = part
                    .FeaturedProducts
                    .ToList();

                var dictionary = products.ToDictionary(p => p.Number, p => _contentManager.BuildDisplay(p.Product, "Summary"));

                return shapeHelper.Parts_FeaturedProductsWidget(Products: dictionary, Amount: part.NumberOfFeaturedProducts);
            });
        }

        protected override DriverResult Editor(FeaturedProductsWidget part, dynamic shapeHelper) {
            return ContentShape("Parts_FeaturedProductsWidget", 
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/FeaturedProductsWidget",
                    Model: part,
                    Prefix: Prefix));
        }

        protected override DriverResult Editor(FeaturedProductsWidget part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}