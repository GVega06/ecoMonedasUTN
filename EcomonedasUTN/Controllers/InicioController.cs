using EcomonedasUTN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcomonedasUTN.Controllers
{
    public class InicioController : Controller
    {
        // GET: Inicio
        public ActionResult Index()
        {
            if (((Usuario)Session["session"]) == null)
            {
                return RedirectToAction("InicioSesion","Usuario");
            }
            ViewBag.User = ViewBag.User;
            return View();
        }
    }
}