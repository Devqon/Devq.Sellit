using System.Linq;
using Devq.Sellit.Services;
using Devq.Sellit.Settings;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;

namespace Devq.Sellit.Drivers
{
    public class TermPartDriver : ContentPartDriver<TermPart> {

        private readonly ICategoryService _categoryService;
        private readonly IContentManager _contentManager;
        private readonly ITaxonomyService _taxonomyService;

        public TermPartDriver(ICategoryService categoryService, 
            IContentManager contentManager, 
            ITaxonomyService taxonomyService) {

            _categoryService = categoryService;
            _contentManager = contentManager;
            _taxonomyService = taxonomyService;
        }

        protected override DriverResult Display(TermPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_TermPart_Children", () => {
                var children = part.Settings.GetModel<ExtraTermPartSettings>().OnlyDirectChildren
                    ? _categoryService.GetDirectChildren(part)
                    : _taxonomyService.GetChildren(part);

                var list = shapeHelper.List();
                list.AddRange(children.Select(c => _contentManager.BuildDisplay(c, "Summary").Term(part)));

                return shapeHelper.Parts_TermPart_Children(ContentItems: list);
            });
        }
    }
}