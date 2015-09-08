using System;
using System.Linq;
using Devq.Sellit.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Core.Common.Models;
using Orchard.Mvc;
using Orchard.Security;
using Orchard.Settings;
using Orchard.UI.Navigation;

namespace Devq.Sellit.Drivers
{
    public class ExtendedProfilePartDriver : ContentPartDriver<ExtendedProfilePart> {

        private readonly IContentManager _contentManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISiteService _siteService;

        public ExtendedProfilePartDriver(IContentManager contentManager, IHttpContextAccessor httpContextAccessor, ISiteService siteService) {
            _contentManager = contentManager;
            _httpContextAccessor = httpContextAccessor;
            _siteService = siteService;
        }

        protected override DriverResult Display(ExtendedProfilePart part, string displayType, dynamic shapeHelper) {
            return Combined(
                ContentShape("Parts_ExtendedProfile",
                    () => shapeHelper.Parts_ExtendedProfile(ContentPart: part)),
                ContentShape("Parts_ExtendedProfile_Items",
                    () => {
                        var pagerParameters = new PagerParameters();
                        var httpContext = _httpContextAccessor.Current();
                        if (httpContext != null)
                        {
                            pagerParameters.Page = Convert.ToInt32(httpContext.Request.QueryString["page"]);
                        }

                        var pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

                        var userId = part.As<IUser>().Id;
                        var items = _contentManager
                            .Query<ProductPart, ProductPartRecord>(VersionOptions.Published)
                            .Where<CommonPartRecord>(c => c.OwnerId == userId);

                        var totalItemCount = items.Count();

                        var pagerShape = pager.PageSize == 0
                            ? null
                            : shapeHelper.Pager(pager)
                                .TotalItemCount(totalItemCount);

                        var pagedItems = items
                            .OrderByDescending<CommonPartRecord>(c => c.CreatedUtc)
                            .Slice(pager.GetStartIndex(), pager.PageSize);

                        var list = shapeHelper.List();
                        list.AddRange(pagedItems.Select(i => _contentManager.BuildDisplay(i, "Summary")));

                        return shapeHelper.Parts_ExtendedProfile_Items(
                            List: list,
                            TotalItemCount: totalItemCount,
                            StartPosition: (pager.Page - 1) * pager.PageSize + 1,
                            EndPosition: pager.Page * pager.PageSize > totalItemCount ? totalItemCount : pager.Page * pager.PageSize,
                            Pager: pagerShape);
                    }));
        }

        protected override DriverResult Editor(ExtendedProfilePart part, dynamic shapeHelper) {
            return ContentShape("Parts_ExtendedProfile_Edit", () => shapeHelper.EditorTemplate(
                TemplateName: "Parts.ExtendedProfile",
                Model: part,
                Prefix: Prefix));
        }

        protected override DriverResult Editor(ExtendedProfilePart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}