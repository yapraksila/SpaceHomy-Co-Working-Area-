using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ourwebpage.Controllers
{
    public class HomeController : Controller
    {
        
        // About sayfası için
        public ActionResult AboutPage()
        {
            return View();
        }

        // MainPage sayfası için 
        public ActionResult MainPage()
        {
            return View();
        }

        public ActionResult ContactPage()
        {  return View();}
    }
}