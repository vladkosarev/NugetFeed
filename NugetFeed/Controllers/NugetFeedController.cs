using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using NuGetFeed.NuGetOData;
using NugetFeed;

namespace NuGetFeed.Controllers
{
    public class NugetFeedController : Controller
    {

        public RssActionResult RSSInclude(string include)
        {
            if (include == null) return RSS(f => false);
            var included = include.Split(',').ToList();
            Func<V1FeedPackage, bool> excludeFunc = p =>
                !included.Exists(i => p.Title.IndexOf(i, StringComparison.OrdinalIgnoreCase) >= 0);
            return RSS(excludeFunc);
        }

        public RssActionResult RSSExclude(string exclude)
        {
            var excluded = exclude == null ? new List<string>() : exclude.Split(',').ToList();
            
            Func<V1FeedPackage, bool> excludeFunc = p =>
                excluded.Exists(i => p.Title.IndexOf(i, StringComparison.OrdinalIgnoreCase) >= 0);
            return RSS(excludeFunc);
        }

        private RssActionResult RSS(Func<V1FeedPackage, bool> exclude)
        {
            var nugetODataUrl = new Uri("http://packages.nuget.org/v1/FeedService.svc");
            var nugetOData = new FeedContext_x0060_1(nugetODataUrl);

            var packages = nugetOData.Packages.OrderByDescending(p => p.Published);
            var nugetFeed = new List<SyndicationItem>();

            foreach (var package in packages)
            {
                if (exclude(package)) continue;

                var syndicationItem = new SyndicationItem(
                    package.Title,
                    package.Description,
                    new Uri(package.ProjectUrl ?? package.GalleryDetailsUrl),
                    package.Id,
                    package.LastUpdated);
                nugetFeed.Add(syndicationItem);
            }

            return new RssActionResult
            {
                Feed = new SyndicationFeed(
                    "NuGet Feed",
                    "NuGet OData feed converted to RSS",
                    new Uri("http://www.nugetfeed.com/"),
                    nugetFeed)
            };
        }


    }
}
