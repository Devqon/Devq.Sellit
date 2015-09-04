using System;
using System.Linq;
using System.Web.Mvc;
using Devq.Sellit.Models;
using Devq.Sellit.Services;
using Devq.Sellit.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.Themes;

namespace Devq.Sellit.Controllers
{
    [Themed]
    public class FeaturedProductsController : Controller {
        private readonly IFeaturedProductService _featuredProductService;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IContentManager _contentManager;

        public FeaturedProductsController(IFeaturedProductService featuredProductService, IWorkContextAccessor workContextAccessor, IContentManager contentManager, IShapeFactory shapeFactory) {
            _featuredProductService = featuredProductService;
            _workContextAccessor = workContextAccessor;
            _contentManager = contentManager;

            Shape = shapeFactory;
        }

        dynamic Shape { get; set; }

        public ActionResult Index() {

            var settings = _workContextAccessor.GetContext().CurrentSite.Get<FeaturedProductsSettingsPart>();
            var now = DateTime.Now;
            var forDate = now;
            var nextDate = _featuredProductService.GetNextTimeLimit();
            if (!nextDate.HasValue) {
                // Set at 23:59:59
                nextDate = forDate.Date.AddSeconds(-1);
            }

            // For tomorrow if after limit
            if (now.TimeOfDay >= nextDate.Value.TimeOfDay) {
                forDate = forDate.AddDays(1);
            }

            var featuredProducts = _featuredProductService
                .GetFeaturedProductsByDate(forDate)
                .List()
                .ToList();

            var items = featuredProducts
                .Select(f => f.ContentItem)
                .ToList();

            // Check if every number is there, otherwise add a new one
            for (var i = 1; i <= settings.NumberOfFeaturedProducts; i++) {
                if (featuredProducts.All(f => f.Number != i)) {
                    var newFeatured = _featuredProductService.CreateFeaturedProduct(forDate, i);
                    items.Add(newFeatured.ContentItem);
                }
            }

            var viewModel = new FeaturedProductsIndexViewModel {
                FeaturedProducts = items.Select(i => _contentManager.BuildDisplay(i)),
                ForDate = forDate,
                TimeLimit = _featuredProductService.BuildTimeLimit(forDate, nextDate.Value)
            };

            return View(viewModel);
        }
    }
}