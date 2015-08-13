using System.Linq;
using Devq.Sellit.Models;
using Devq.Sellit.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Taxonomies.Services;

namespace Devq.Sellit.Drivers
{
    public class LevelTermsWidgetDriver : ContentPartDriver<LevelTermsWidgetPart> {

        private readonly IContentManager _contentManager;
        private readonly ITaxonomyService _taxonomyService;

        public LevelTermsWidgetDriver(IContentManager contentManager, ITaxonomyService taxonomyService) {
            _contentManager = contentManager;
            _taxonomyService = taxonomyService;
        }

        protected override DriverResult Display(LevelTermsWidgetPart part, string displayType, dynamic shapeHelper) {
            return ContentShape("Parts_LevelTermsWidget", () => {

                var list = shapeHelper.List();
                list.AddRange(part.LevelTerms.Select(t => _contentManager.BuildDisplay(t, "Summary")));
                
                return shapeHelper.Parts_LevelTermsWidget(ContentItems: list);
            });
        }

        // GET
        protected override DriverResult Editor(LevelTermsWidgetPart part, dynamic shapeHelper) {
            return ContentShape("Parts_LevelTermsWidget_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/LevelTermsWidget",
                    Model: BuildViewModel(part),
                    Prefix: Prefix));
        }

        // POST
        protected override DriverResult Editor(LevelTermsWidgetPart part, IUpdateModel updater, dynamic shapeHelper) {

            var viewModel = new LevelTermsWidgetViewEditModel();

            if (updater.TryUpdateModel(viewModel, Prefix, null, null)) {
                part.ForTaxonomy = viewModel.Taxonomy;
            }
            return Editor(part, shapeHelper);
        }

        private LevelTermsWidgetViewEditModel BuildViewModel(LevelTermsWidgetPart part)
        {
            return new LevelTermsWidgetViewEditModel {
                Taxonomies = _taxonomyService.GetTaxonomies(),
                Taxonomy = part.ForTaxonomy
            };
        }
    }
}