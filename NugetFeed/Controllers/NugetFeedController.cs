using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using NugetFeed.NugetOData;

namespace NugetRSS.Controllers
{
    public class NugetFeedController : Controller
    {

        public RssActionResult RSSInclude(string include)
        {
            var included = include.Split('_').ToList();
            Func<V1FeedPackage, bool> ignoreFunc = p =>
                !included.Exists(i => p.Title.IndexOf(i, StringComparison.OrdinalIgnoreCase) >= 0);
            return RSS(ignoreFunc);
        }

        public RssActionResult RSSIgnore(string ignore)
        {
            var ignored = ignore == null ? new List<string>() : ignore.Split('_').ToList();
            Func<V1FeedPackage, bool> ignoreFunc = p =>
                ignored.Exists(i => p.Title.IndexOf(i, StringComparison.OrdinalIgnoreCase) >= 0);
            return RSS(ignoreFunc);
        }

        private RssActionResult RSS(Func<V1FeedPackage, bool> ignore)
        {
            var nugetODataUrl = new Uri("http://packages.nuget.org/v1/FeedService.svc");
            var nugetOData = new FeedContext_x0060_1(nugetODataUrl);

            var packages = nugetOData.Packages.OrderByDescending(p => p.Published);
            var packagesFeed = new List<SyndicationItem>();

            foreach (var package in packages)
            {
                if (ignore(package)) continue;

                var syndicationItem = new SyndicationItem(
                    package.Title,
                    package.Description,
                    new Uri(package.ProjectUrl ?? package.GalleryDetailsUrl),
                    package.Id,
                    package.LastUpdated);
                packagesFeed.Add(syndicationItem);
            }

            return new RssActionResult
            {
                Feed = new SyndicationFeed(
                    "Nuget Feed",
                    "Nuget OData feed converted to RSS",
                    new Uri("http://www.nugetfeed.com/"),
                    packagesFeed)
            };
        }


    }
}
