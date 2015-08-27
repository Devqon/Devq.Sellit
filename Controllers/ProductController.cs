using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Devq.Sellit.Models;
using Devq.Sellit.Services;
using Devq.Sellit.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Mvc.Html;
using Orchard.Taxonomies.Fields;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;
using Orchard.Themes;
using Orchard.UI.Notify;

namespace Devq.Sellit.Controllers
{
    [ValidateInput(false), Themed]
    public class ProductController : Controller, IUpdateModel {
        private readonly IContentManager _contentManager;
        private readonly ITransactionManager _transactionManager;
        private readonly IOrchardServices _orchardServices;
        private readonly IProductService _productService;
        private readonly ITaxonomyService _taxonomyService;

        public ProductController(IContentManager contentManager, 
            ITransactionManager transactionManager, 
            IOrchardServices orchardServices, 
            IProductService productService, ITaxonomyService taxonomyService) {

            _contentManager = contentManager;
            _transactionManager = transactionManager;
            _orchardServices = orchardServices;
            _productService = productService;
            _taxonomyService = taxonomyService;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        // Step 1, choose the category
        [HttpGet]
        public ActionResult Create() {
            var product = _contentManager.New(Constants.ProductName);

            if (!_orchardServices.Authorizer.Authorize(Permissions.AddProduct, product, T("Not allowed to create a product"))) {
                return new HttpUnauthorizedResult();
            }

            var model = _contentManager.BuildEditor(product);

            return View(model);
        }

        [HttpPost, ActionName("Create")]
        [FormValueRequired("submit.Category")]
        public ActionResult CreateCategoryPost() {
            var product = _contentManager.New(Constants.ProductName);

            if (!_orchardServices.Authorizer.Authorize(Permissions.AddProduct, product, T("Not allowed to create a product")))
            {
                return new HttpUnauthorizedResult();
            }

            var updated = _contentManager.UpdateEditor(product, this);

            _transactionManager.Cancel();

            var productPart = product.As<ProductPart>();
            if (string.IsNullOrEmpty(productPart.Category)) {
                return View(updated);
            }

            // Get extensions part
            var toWeld = _productService.GetNewExtensionPart(productPart.Category);
            // Weld to product
            if (toWeld != null) {
                product.Weld(toWeld);
            }

            var model = _contentManager.BuildEditor(product);

            TempData["ProductCategory"] = productPart.Category;

            return View(model);
        }

        // Step 2, fill in details
        [HttpPost, ActionName("Create")]
        [FormValueRequired("submit.Create")]
        public ActionResult CreatePost() {

            var product = _contentManager.New(Constants.ProductName);

            if (!_orchardServices.Authorizer.Authorize(Permissions.AddProduct, product, T("Not allowed to create a product")))
            {
                return new HttpUnauthorizedResult();
            }

            var category = Request.Form["ProductCategory"];

            // Get extensions part
            var toWeld = _productService.GetNewExtensionPart(category);
            // Weld to product
            if (toWeld != null)
            {
                product.Weld(toWeld);
            }

            var model = _contentManager.UpdateEditor(product, this);

            if (!ModelState.IsValid) {
                _transactionManager.Cancel();
                return View(model);
            }

            product.As<ProductPart>().Category = category;

            // Valid
            _contentManager.Create(product);
            var taxonomy = _taxonomyService.GetTaxonomyByName(Constants.CategoryTaxonomyName);
            var term = _taxonomyService.GetTermByName(taxonomy.Id, category);
            _taxonomyService.UpdateTerms(product, new List<TermPart> {term}, Constants.CategoryTaxonomyName);
            return Redirect(Url.ItemDisplayUrl(product));
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            var contentItem = _contentManager.Get(id);

            // Check if can go through, only if has permissions and the content type is actually a product
            if (contentItem == null || contentItem.ContentType != Constants.ProductName)
            {
                return HttpNotFound();
            }

            if (!_orchardServices.Authorizer.Authorize(Permissions.AddProduct, contentItem)) {
                return new HttpUnauthorizedResult();
            }

            // Build editor to display in frontend
            var model = _contentManager.BuildEditor(contentItem);

            return View((object) model);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int id)
        {
            var contentItem = _contentManager.Get(id);

            // Check if can go through, only if has permissions and the content type is actually a product
            if (contentItem == null || !_orchardServices.Authorizer.Authorize(Permissions.AddProduct) || contentItem.ContentType != Constants.ProductName)
            {
                return HttpNotFound();
            }

            // Try update editor
            var model = _contentManager.UpdateEditor(contentItem, this);
            if (!ModelState.IsValid)
            {
                // Cancel not valid
                _transactionManager.Cancel();
                return View((object) model);
            }

            return RedirectToAction("Index", new RouteValueDictionary { { "id", contentItem.Id } });
        }

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }
}