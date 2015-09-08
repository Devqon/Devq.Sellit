using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Devq.Bids.Models;
using Devq.Bids.Services;
using Devq.Sellit.Models;
using Devq.Sellit.Services;
using Devq.Sellit.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Core.Title.Models;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Mvc.Html;
using Orchard.Themes;
using Orchard.UI.Notify;

namespace Devq.Sellit.Controllers
{
    [Themed]
    public class FeaturedProductsController : Controller {
        private readonly IFeaturedProductService _featuredProductService;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IContentManager _contentManager;
        private readonly IProductService _productService;
        private readonly IBidService _bidService;
        private readonly IOrchardServices _orchardServices;

        public FeaturedProductsController(IFeaturedProductService featuredProductService, IWorkContextAccessor workContextAccessor, IContentManager contentManager, IShapeFactory shapeFactory, IProductService productService, IBidService bidService, IOrchardServices orchardServices) {
            _featuredProductService = featuredProductService;
            _workContextAccessor = workContextAccessor;
            _contentManager = contentManager;

            Shape = shapeFactory;
            _productService = productService;
            _bidService = bidService;
            _orchardServices = orchardServices;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        dynamic Shape { get; set; }

        public ActionResult Index() {

            var settings = _workContextAccessor.GetContext().CurrentSite.Get<FeaturedProductsSettingsPart>();
            var now = DateTime.UtcNow;
            var forDate = now.AddDays(1);
            var nextDate = _featuredProductService.GetNextTimeLimit();
            if (!nextDate.HasValue) {
                // Set at 23:59:59
                nextDate = now.Date.AddSeconds(-1);
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
                TimeLimit = _featuredProductService.BuildTimeLimit(nextDate.Value, nextDate.Value)
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Pay(int id) {

            var featuredProduct = _contentManager.Get<FeaturedProductPart>(id);
            if (featuredProduct == null) {
                return HttpNotFound();
            }

            var user = _workContextAccessor.GetContext().CurrentUser;
            if (user == null) {
                return HttpNotFound();
            }

            var heighestBid = _bidService.GetHeighestBid(featuredProduct.Id);

            // Check if the user is the heighest bider
            if (heighestBid != null && heighestBid.Owner.Id != user.Id) {
                _orchardServices.Notifier.Information(T("I don't think you'll want to pay for someone else's featured product :)"));
                return RedirectToAction("Index");
            }

            // Already active so already paid for
            if (featuredProduct.Active) {
                _orchardServices.Notifier.Information(T("This featured product is already active"));
                return Redirect(Url.ItemDisplayUrl(featuredProduct.Product));
            }

            var settings = _workContextAccessor.GetContext().CurrentSite.As<FeaturedProductsSettingsPart>();
            if (featuredProduct.Date < _featuredProductService.BuildTimeLimit(DateTime.UtcNow, settings.TimeLimit.GetValueOrDefault())) {
                return HttpNotFound();
            }

            var userProducts = _productService
                .GetProductsQuery()
                .Where<CommonPartRecord>(c => c.OwnerId == user.Id)
                .List();

            var viewModel = new FeaturedProductsPayViewModel {
                Products = userProducts.Select(up => new SelectListItem{ Text = up.As<TitlePart>().Title, Value = up.Id.ToString() }),
                Amount = heighestBid == null ? 0 : heighestBid.BidPrice,
                FeaturedProduct = featuredProduct
            };

            return View(viewModel);
        }

        private decimal HeighestBid(int id) {
            var heighestBid = _bidService.GetHeighestBid(id);
            if (heighestBid == null)
                return 0;

            return heighestBid.BidPrice;
        }
    }

    public class FeaturedProductsPayViewModel {
        public IEnumerable<SelectListItem> Products { get; set; }
        public decimal Amount { get; set; }
        public FeaturedProductPart FeaturedProduct { get; set; }
        public int ProductId { get; set; }
    }
}