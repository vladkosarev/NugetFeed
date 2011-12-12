using System.ServiceModel.Syndication;
using System.Xml;
using System.Web.Mvc;

namespace NugetFeed
{
    public class RssActionResult : ActionResult
    {
        public SyndicationFeed Feed { get; set; }

        public override void ExecuteResult(ControllerContext controllerContext)
        {
            controllerContext.HttpContext.Response.ContentType = "application/rss+xml";
            var rssFormatter = new Rss20FeedFormatter(Feed);
            using (var xmlWriter = XmlWriter.Create(controllerContext.HttpContext.Response.Output))
            {
                rssFormatter.WriteTo(xmlWriter);
            }
        }
    }
}