using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Devq.Sellit.Services;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.Core.Common.Models;
using Orchard.Core.Contents.Settings;
using Orchard.Core.Contents.ViewModels;
using Orchard.DisplayManagement;
using Orchard.Localization.Services;
using Orchard.Settings;
using Orchard.UI.Admin;
using Orchard.UI.Navigation;

namespace Devq.Sellit.Controllers
{
    [Admin]
    public class ProductAdminController : Controller {
        private readonly IContentManager _contentManager;
        private readonly IProductService _productService;
        private readonly ISiteService _siteService;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ICultureManager _cultureManager;
        private readonly ICultureFilter _cultureFilter;

        public ProductAdminController(IProductService productService, IShapeFactory shapeFactory, ISiteService siteService, IContentManager contentManager, IContentDefinitionManager contentDefinitionManager, ICultureManager cultureManager, ICultureFilter cultureFilter, IOrchardServices services) {
            Shape = shapeFactory;
            _productService = productService;
            _siteService = siteService;
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _cultureManager = cultureManager;
            _cultureFilter = cultureFilter;

            Services = services;
        }

        public dynamic Shape { get; set; }
        public IOrchardServices Services { get; set; }

        public ActionResult List(ListContentsViewModel model, PagerParameters pagerParameters)
        {
            var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

            var versionOptions = VersionOptions.Latest;
            switch (model.Options.ContentsStatus)
            {
                case ContentsStatus.Published:
                    versionOptions = VersionOptions.Published;
                    break;
                case ContentsStatus.Draft:
                    versionOptions = VersionOptions.Draft;
                    break;
                case ContentsStatus.AllVersions:
                    versionOptions = VersionOptions.AllVersions;
                    break;
                default:
                    versionOptions = VersionOptions.Latest;
                    break;
            }

            var query = _productService
                .GetProductsQuery()
                .ForVersion(versionOptions);

            if (!string.IsNullOrEmpty(model.TypeName))
            {
                var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(model.TypeName);
                if (contentTypeDefinition == null)
                    return HttpNotFound();

                model.TypeDisplayName = !string.IsNullOrWhiteSpace(contentTypeDefinition.DisplayName)
                                            ? contentTypeDefinition.DisplayName
                                            : contentTypeDefinition.Name;
                query = query.ForType(model.TypeName);
            }

            switch (model.Options.OrderBy)
            {
                case ContentsOrder.Modified:
                    //query = query.OrderByDescending<ContentPartRecord, int>(ci => ci.ContentItemRecord.Versions.Single(civr => civr.Latest).Id);
                    query = query.OrderByDescending<CommonPartRecord>(cr => cr.ModifiedUtc);
                    break;
                case ContentsOrder.Published:
                    query = query.OrderByDescending<CommonPartRecord>(cr => cr.PublishedUtc);
                    break;
                case ContentsOrder.Created:
                    //query = query.OrderByDescending<ContentPartRecord, int>(ci => ci.Id);
                    query = query.OrderByDescending<CommonPartRecord>(cr => cr.CreatedUtc);
                    break;
            }

            model.Options.SelectedFilter = model.TypeName;
            model.Options.FilterOptions = GetListableTypes(false)
                .Select(ctd => new KeyValuePair<string, string>(ctd.Name, ctd.DisplayName))
                .ToList().OrderBy(kvp => kvp.Value);

            model.Options.Cultures = _cultureManager.ListCultures();

            var maxPagedCount = _siteService.GetSiteSettings().MaxPagedCount;
            if (maxPagedCount > 0 && pager.PageSize > maxPagedCount)
                pager.PageSize = maxPagedCount;
            var pagerShape = Shape.Pager(pager).TotalItemCount(maxPagedCount > 0 ? maxPagedCount : query.Count());
            var pageOfContentItems = query.Slice(pager.GetStartIndex(), pager.PageSize).ToList();

            var list = Shape.List();
            list.AddRange(pageOfContentItems.Select(ci => _contentManager.BuildDisplay(ci, "SummaryAdmin")));

            var viewModel = Shape.ViewModel()
                .ContentItems(list)
                .Pager(pagerShape)
                .Options(model.Options)
                .TypeDisplayName(model.TypeDisplayName ?? "");

            return View(viewModel);
        }

        private IEnumerable<ContentTypeDefinition> GetCreatableTypes(bool andContainable)
        {
            return _contentDefinitionManager.ListTypeDefinitions().Where(ctd =>
                Services.Authorizer.Authorize(Orchard.Core.Contents.Permissions.EditContent, _contentManager.New(ctd.Name)) &&
                ctd.Settings.GetModel<ContentTypeSettings>().Creatable &&
                (!andContainable || ctd.Parts.Any(p => p.PartDefinition.Name == "ContainablePart")));
        }

        private IEnumerable<ContentTypeDefinition> GetListableTypes(bool andContainable)
        {
            return _productService
                .GetCategories()
                .Where(ctd =>
                    Services.Authorizer.Authorize(Orchard.Core.Contents.Permissions.EditContent, _contentManager.New(ctd.Name)) &&
                    ctd.Settings.GetModel<ContentTypeSettings>().Listable &&
                    (!andContainable || ctd.Parts.Any(p => p.PartDefinition.Name == "ContainablePart"))
                );
        }
    }
}