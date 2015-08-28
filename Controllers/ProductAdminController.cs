using System;
using System.Web.Mvc;
using Devq.Sellit.Services;
using Orchard.DisplayManagement;
using Orchard.Settings;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;

namespace Devq.Sellit.Controllers
{
    [Admin]
    public class ProductAdminController : Controller
    {
        private dynamic Shape { get; set; }
        private readonly IProductService _productService;
        private readonly ISiteService _siteService;
 
        public ProductAdminController(IProductService productService, IShapeFactory shapeFactory, ISiteService siteService) {
            Shape = shapeFactory;
            _productService = productService;
            _siteService = siteService;
        }
 
        public ActionResult Index(PagerParameters pagerParameters) {
 
            return View();
        }
    }
}