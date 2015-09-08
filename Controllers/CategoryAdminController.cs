using System;
using System.Linq;
using System.Web.Mvc;
using Devq.Sellit.Services;
using Devq.Sellit.ViewModels;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentTypes.Services;
using Orchard.Localization;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using Orchard.Utility.Extensions;

namespace Devq.Sellit.Controllers
{
    [Admin]
    public class CategoryAdminController : Controller {

        private readonly IOrchardServices _orchardServices;
        private readonly IProductService _productService;
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionService _contentDefinitionService;
        private readonly IContentDefinitionEditorEvents _contentDefinitionEditorEvents;

        public CategoryAdminController(IProductService productService, IOrchardServices orchardServices, IContentManager contentManager, IContentDefinitionService contentDefinitionService, IContentDefinitionEditorEvents contentDefinitionEditorEvents) {
            _productService = productService;
            _orchardServices = orchardServices;
            _contentManager = contentManager;
            _contentDefinitionService = contentDefinitionService;
            _contentDefinitionEditorEvents = contentDefinitionEditorEvents;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        [HttpGet]
        public ActionResult Index(PagerParameters pagerParameters) {

            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageProducts)) {
                return new HttpUnauthorizedResult();
            }

            var categories = _productService.GetProductTypes();

            var pager = new Pager(_orchardServices.WorkContext.CurrentSite, pagerParameters);
            var pagerShape = _orchardServices.New.Pager(pager).TotalItemCount(categories.Count());
            if (pager.PageSize != 0)
            {
                categories = categories.Skip(pager.GetStartIndex()).Take(pager.PageSize);
            }

            var entries = categories.Select(CreateCategoryEntry).ToList();
            var model = new CategoryAdminIndexViewModel { Pager = pagerShape, Categories = entries };

            return View(model);
        }

        [HttpGet]
        public ActionResult Create() {

            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageProductTypes)) {
                return new HttpUnauthorizedResult();
            }

            var viewModel = new CreateProductTypeViewModel();

            return View(viewModel);
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost(CreateProductTypeViewModel viewModel) {

            if (!_orchardServices.Authorizer.Authorize(Permissions.ManageProductTypes)) {
                return new HttpUnauthorizedResult();
            }

            viewModel.DisplayName = viewModel.DisplayName ?? string.Empty;
            if (string.IsNullOrEmpty(viewModel.DisplayName)) {
                _orchardServices.TransactionManager.Cancel();
                ModelState.AddModelError("DisplayName", T("Name is mandatory").Text);
                return View(viewModel);
            }

            var result = _productService.CreateProductType(viewModel.DisplayName);
            if (!result) {
                _orchardServices.TransactionManager.Cancel();
                _orchardServices.Notifier.Error(T("Could not create {0}, the type already exists", viewModel.DisplayName));
                return View(viewModel);
            }

            var name = viewModel.DisplayName.ToSafeName();

            _orchardServices.Notifier.Information(T("Product type {0} successfully created", viewModel.DisplayName));

            return RedirectToAction("AddPartsTo", "Admin", new { Area = "Orchard.ContentTypes", id = name });
        }
        
        private static CategoryEntry CreateCategoryEntry(Tuple<string, string> category)
        {
            return new CategoryEntry
            {
                DisplayName = category.Item2,
                Name = category.Item1,
                IsChecked = false,
            };
        }
    }
}