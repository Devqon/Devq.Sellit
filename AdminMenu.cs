using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard;
using Orchard.Localization;
using Orchard.UI.Navigation;

namespace Devq.Sellit
{
    public class AdminMenu : INavigationProvider
    {
        public string MenuName
        {
            get { return "admin"; }
        }

        public AdminMenu()
        {
            T = NullLocalizer.Instance;
        }

        private Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder

                // Image set
                .AddImageSet("Sellit")

                // "Sellit"
                .Add(item => item

                    .Caption(T("Sellit"))
                    .Position("2")
                    .LinkToFirstChild(false)

                    // "Products"
                    .Add(subItem => subItem
                        .Caption(T("Products"))
                        .Position("2.1")
                        .Action("List", "ProductAdmin", new { area = "Devq.Sellit" }))

                    // Categories
                    .Add(tab => tab
                        .Caption(T("Categories"))
                        .Position("2.5")
                        .Action("Index", "CategoryAdmin", new { area = "Devq.Sellit" }))

                    // "Customers"
                    .Add(subItem => subItem
                        .Caption(T("Customers"))
                        .Position("2.8")
                        .Action("Index", "CustomerAdmin", new { area = "Devq.Sellit" }))
                );
        }
    }
}