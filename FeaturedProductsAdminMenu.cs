using Orchard.Environment.Extensions;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace Devq.Sellit
{
    [OrchardFeature("Devq.FeaturedProducts")]
    public class FeaturedProductsAdminMenu : INavigationProvider
    {
        public FeaturedProductsAdminMenu()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public string MenuName
        {
            get { return "admin"; }
        }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T("Sellit"),
                menu => menu
                    .Add(T("Featured Products"), "2.5", 
                        item => item
                            .Action("Manage", "FeaturedProductsAdmin", new { area = "Devq.Sellit" }))
                );

        }
    }
}