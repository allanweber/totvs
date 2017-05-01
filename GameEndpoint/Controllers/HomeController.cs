using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameEndpoint.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //Seleciona a Url base configurada
            ViewBag.URL = string.Concat(Request.Url.Scheme, "://", Request.Url.Authority, Request.ApplicationPath.TrimEnd('/'), "/", "api/game");

            return View();
        }
    }
}