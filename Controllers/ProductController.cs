using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Devq.Sellit.Services;
using Devq.Sellit.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Mvc.Html;
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
        private readonly ICategoryService _categoryService;

        public ProductController(IContentManager contentManager, 
            ITransactionManager transactionManager, 
            IOrchardServices orchardServices, 
            IProductService productService, 
            ICategoryService categoryService) {

            _contentManager = contentManager;
            _transactionManager = transactionManager;
            _orchardServices = orchardServices;
            _productService = productService;
            _categoryService = categoryService;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        [HttpGet]
        public ActionResult Create()
        {
            if (!_orchardServices.Authorizer.Authorize(Permissions.AddProduct, T("Not allowed to create a product")))
            {
                return new HttpUnauthorizedResult();
            }

            var categories = _categoryService
                .GetTopLevelTerms(Constants.CategoryTaxonomyName)
                .OrderBy(c => c.Name);

            var model = new SelectCategoryViewModel {
                Categories = categories.ToDictionary(c => c.Id, c => c.Name)
            };

            return View("ChooseCategory", model);
        }

        [HttpPost, ActionName("Create")]
        [FormValueRequired("submit.Category")]
        public ActionResult CreateCategoryPost(SelectCategoryViewModel model) {

            var type = _productService.GetTypeByCategory(model.SelectedCategory);
            if (type == null) {
                _orchardServices.Notifier.Error(T("Something went wrong.., type with id {0} does not exist", model.SelectedCategory));
                return RedirectToAction("Create");
            }
            var product = _contentManager.New(type.Name);

            if (!_orchardServices.Authorizer.Authorize(Permissions.AddProduct, product, T("Not allowed to create a product"))) {
                return new HttpUnauthorizedResult();
            }

            if (string.IsNullOrEmpty(model.SelectedCategory)) {
                return View("ChooseCategory", model);
            }

            var editor = _contentManager.BuildEditor(product).Category(type.Name).CategoryName(type.DisplayName);

            return View(editor);
        }

        [HttpPost, ActionName("Create")]
        [FormValueRequired("submit.Save")]
        public ActionResult CreatePost(string type) {

            var contentType = _contentManager.GetContentTypeDefinitions().FirstOrDefault(c => c.Name == type);
            if (contentType == null) {
                _orchardServices.Notifier.Error(T("Type {0} does not exist", type));
                return RedirectToAction("Create");
            }

            var product = _contentManager.New(type);

            if (!_orchardServices.Authorizer.Authorize(Permissions.AddProduct, product, T("Not allowed to create a product")))
            {
                return new HttpUnauthorizedResult();
            }

            var model = _contentManager.UpdateEditor(product, this);
            _contentManager.Create(product);

            if (!ModelState.IsValid) {
                _transactionManager.Cancel();
                return View(model);
            }

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