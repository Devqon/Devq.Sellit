using System.Web.Mvc;
using System.Web.Routing;
using Devq.Sellit.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Data;
using Orchard.Localization;
using Orchard.Themes;
using Orchard.UI.Notify;

namespace Devq.Sellit.Controllers
{
    [ValidateInput(false), Themed]
    public class ProductController : Controller, IUpdateModel {
        private readonly IContentManager _contentManager;
        private readonly ITransactionManager _transactionManager;
        private readonly IOrchardServices _orchardServices;

        public ProductController(IContentManager contentManager, 
            ITransactionManager transactionManager, 
            IOrchardServices orchardServices) {

            _contentManager = contentManager;
            _transactionManager = transactionManager;
            _orchardServices = orchardServices;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        [HttpGet]
        public ActionResult Index(int id) {
            var contentItem = _contentManager.Get(id);

            // Check if can go through, only if has permissions and the content type is actually a product
            if (contentItem == null || contentItem.ContentType != Constants.ProductName)
            {
                return HttpNotFound();
            }

            var model = _contentManager.BuildDisplay(contentItem);

            return View((object) model);
        }

        [HttpGet]
        public ActionResult Create() {

            // New product
            var product = _contentManager.New(Constants.ProductName);

            if (!_orchardServices.Authorizer.Authorize(Permissions.AddProduct, product, T("Not allowed to create a product")))
            {
                return new HttpUnauthorizedResult();
            }

            var model = _contentManager.BuildEditor(product);

            return View((object) model);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost() {

            var product = _contentManager.New(Constants.ProductName);

            if (!_orchardServices.Authorizer.Authorize(Permissions.AddProduct, product, T("Not allowed to create a product"))) {
                return new HttpUnauthorizedResult();
            }

            _contentManager.Create(product);
            var model = _contentManager.UpdateEditor(product, this);

            if (!ModelState.IsValid) {
                _transactionManager.Cancel();
                return View(model);
            }

            _orchardServices.Notifier.Information(T("Your {0} has been created", product.TypeDefinition.DisplayName));
            return RedirectToAction("Index", new {id = product.Id});
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