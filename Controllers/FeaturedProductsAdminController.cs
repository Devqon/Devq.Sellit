using System;
using System.Linq;
using System.Web.Mvc;
using Devq.Sellit.Services;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions;
using Orchard.UI.Admin;

namespace Devq.Sellit.Controllers
{
    [OrchardFeature("Devq.FeaturedProducts")]
    [Admin]
    public class FeaturedProductsAdminController : Controller {

        private readonly IFeaturedProductService _featuredProductService;
        private readonly IContentManager _contentManager;

        public FeaturedProductsAdminController(IFeaturedProductService featuredProductService, IContentManager contentManager, IShapeFactory shapeFactory) {
            _featuredProductService = featuredProductService;
            _contentManager = contentManager;

            Shape = shapeFactory;
        }

        dynamic Shape { get; set; }

        public ActionResult Manage(FeaturedProductsAdminIndexViewModel model)
        {
            if (model.Date == DateTime.MinValue) {
                model.Date = DateTime.Now;
            }

            var featuredProducts = _featuredProductService
                .GetFeaturedProductsByDate(model.Date);

            var shapes = featuredProducts
                .List()
                .Select(fp => _contentManager.BuildDisplay(fp, "SummaryAdmin"));

            var viewModel = Shape.ViewModel()
                .Shapes(shapes)
                .Date(model.Date);

            return View(viewModel);
        }
    }

    public class FeaturedProductsAdminIndexViewModel {
        public DateTime Date { get; set; }
    }
}