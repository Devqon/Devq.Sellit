using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace Devq.Sellit
{
    public class Routes : IRouteProvider
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
                        "Devq.Sellit", // this is the name of the page url
                        new RouteValueDictionary {
                            {"area", "Devq.Sellit"}, // this is the name of your module
                            {"controller", "Product"},
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