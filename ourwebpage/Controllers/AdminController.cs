using System.Web.Mvc;
using ourwebpage.Models;

namespace ourwebpage.Controllers
{
    [Authorize(Roles = MyConstants.RoleAdmin)]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}