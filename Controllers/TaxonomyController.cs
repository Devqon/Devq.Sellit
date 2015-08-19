using System.Linq;
using System.Web.Mvc;
using Devq.Sellit.Services;

namespace Devq.Sellit.Controllers
{
    public class TaxonomyController : Controller {
        private readonly ICategoryService _categoryService;

        public TaxonomyController(ICategoryService categoryService) {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get the children of a term
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetChildren(int id) {

            var children = _categoryService.GetDirectChildren(id);

            return Json(new {
                terms = children.Select(t => new {t.Id, t.Name})
            });
        }
    }
}