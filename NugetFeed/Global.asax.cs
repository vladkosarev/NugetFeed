using System.Web.Mvc;
using System.Web.Routing;

namespace NugetFeed
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Default",
                "",
                new { controller = "Default", action = "Index" }
            );

            routes.MapRoute(
                "RSS",
                "rss",
                new { controller = "NugetFeed", action = "RSSIgnore" }
            );

            routes.MapRoute(
                "RSSIgnore",
                "rss/ignore/{ignore}",
                new { controller = "NugetFeed", action = "RSSIgnore", ignore = UrlParameter.Optional }
            );

            routes.MapRoute(
                "RSSInclude",
                "rss/include/{include}",
                new { controller = "NugetFeed", action = "RSSInclude", include = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}