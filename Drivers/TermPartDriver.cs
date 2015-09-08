using System;
using System.Linq;
using Devq.Sellit.Services;
using Devq.Sellit.Settings;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Mvc;
using Orchard.Settings;
using Orchard.Taxonomies.Models;
using Orchard.Taxonomies.Services;
using Orchard.Taxonomies.Settings;
using Orchard.UI.Navigation;

namespace Devq.Sellit.Drivers
{
    public class TermPartDriver : ContentPartDriver<TermPart> {

        private readonly ICategoryService _categoryService;
        private readonly IContentManager _contentManager;
        private readonly ITaxonomyService _taxonomyService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISiteService _siteService;
        
        public TermPartDriver(ICategoryService categoryService, 
            IContentManager contentManager, 
            ITaxonomyService taxonomyService, IHttpContextAccessor httpContextAccessor, ISiteService siteService) {

            _categoryService = categoryService;
            _contentManager = contentManager;
            _taxonomyService = taxonomyService;
            _httpContextAccessor = httpContextAccessor;
            _siteService = siteService;
        }

        protected override DriverResult Display(TermPart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_TermPart_Children", () => {
                    var children = part.Settings.GetModel<ExtraTermPartSettings>().OnlyDirectChildren
                        ? _categoryService.GetDirectChildren(part)
                        : _taxonomyService.GetChildren(part);

                    var list = shapeHelper.List();
                    list.AddRange(children.Select(c => _contentManager.BuildDisplay(c, "Summary").Term(c)));

                    return shapeHelper.Parts_TermPart_Children(ContentItems: list);
                }),
                // Use this one instead of the default TermPart,
                // This only displays the direct linked items, instead of the
                // child items too
                ContentShape("Parts_TermPart_DirectItems", () => {
                    var pagerParameters = new PagerParameters();
                    var httpContext = _httpContextAccessor.Current();
                    if (httpContext != null)
                    {
                        pagerParameters.Page = Convert.ToInt32(httpContext.Request.QueryString["page"]);
                    }

                    var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);
                    var taxonomy = _taxonomyService.GetTaxonomy(part.TaxonomyId);
                    var totalItemCount = _taxonomyService.GetContentItemsCount(part);

                    var partSettings = part.Settings.GetModel<TermPartSettings>();
                    if (partSettings != null && partSettings.OverrideDefaultPagination)
                    {
                        pager.PageSize = partSettings.PageSize;
                    }

                    var childDisplayType = partSettings != null &&
                                           !String.IsNullOrWhiteSpace(partSettings.ChildDisplayType)
                        ? partSettings.ChildDisplayType
                        : "Summary";
                    // asign Taxonomy and Term to the content item shape (Content) in order to provide 
                    // alternates when those content items are displayed when they are listed on a term
                    var termContentItems = _categoryService.GetDirectContentItems(part, pager.GetStartIndex(), pager.PageSize)
                        .Select(c => _contentManager.BuildDisplay(c, childDisplayType).Taxonomy(taxonomy).Term(part));

                    var list = shapeHelper.List();

                    list.AddRange(termContentItems);

                    var pagerShape = pager.PageSize == 0
                        ? null
                        : shapeHelper.Pager(pager)
                            .TotalItemCount(totalItemCount)
                            .Taxonomy(taxonomy)
                            .Term(part);

                    return shapeHelper.Parts_TermPart(ContentItems: list, Taxonomy: taxonomy, Pager: pagerShape);
                }));
        }
    }
}