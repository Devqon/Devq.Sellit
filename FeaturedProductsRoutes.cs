using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Environment.Extensions;
using Orchard.Mvc.Routes;

namespace Devq.Sellit
{
    [OrchardFeature("Devq.FeaturedProducts")]
    public class FeaturedProductsRoutes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "FeaturedProducts", // this is the name of the page url
                        new RouteValueDictionary {
                            {"area", "Devq.Sellit"}, // this is the name of your module
                            {"controller", "FeaturedProducts"},
                            {"action", "Index"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "Devq.Sellit"} // this is the name of your module
                        },
                        new MvcRouteHandler())
                }
            };
        }
    }
}