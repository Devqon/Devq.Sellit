using System.Collections.Generic;
using Devq.Sellit.Models;
using Devq.Sellit.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace Devq.Sellit.Handlers
{
    public class LevelTermsWidgetPartHandler : ContentHandler {
        private readonly ICategoryService _categoryService;

        public LevelTermsWidgetPartHandler(
            IRepository<LevelTermsWidgetPartRecord> repository,
            ICategoryService categoryService) {

            _categoryService = categoryService;

            Filters.Add(StorageFilter.For(repository));

            OnLoading<LevelTermsWidgetPart>((ctx, part) => LoadTerms(part));
        }

        private void LoadTerms(LevelTermsWidgetPart part) {
            
            part._levelTermsField.Loader(prt => {

                if (string.IsNullOrEmpty(part.ForTaxonomy))
                    return new List<IContent>();

                return _categoryService.GetTopLevelTerms(part.ForTaxonomy);
            });
        }
    }
}