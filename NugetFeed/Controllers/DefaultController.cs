using System.Web.Mvc;

namespace NugetRSS.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
