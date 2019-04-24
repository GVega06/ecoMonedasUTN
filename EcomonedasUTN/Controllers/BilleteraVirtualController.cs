using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EcomonedasUTN.Models;

namespace EcomonedasUTN.Controllers
{
    public class BilleteraVirtualController : Controller
    {
        private ecoMonedaModel db = new ecoMonedaModel();

        // GET: BilleteraVirtual
        public ActionResult Index()
        {
            Usuario user = (Usuario)Session["session"];
            if (user == null)
            {
                return RedirectToAction("InicioSesion","Usuario");
            }
            var lista = db.BilleteraVirtual.Where(x => x.idUsuario.Equals(user.email));
            BilleteraVirtual v = null;
           var h = db.historial.Where(x => x.idUsuario.Equals(user.email));
            foreach(BilleteraVirtual item in lista)
            {
                if (item.idUsuario.Equals(user.email))
                {
                    v = item;
                }
            }
            if(v == null)
            {
                v = new BilleteraVirtual();
                v.idUsuario = user.email;
                v.total = 0;
                db.BilleteraVirtual.Add(v);
                db.SaveChanges();
            }


            double canje = 0;
            double totalCanjes = 0;
            foreach (var item in h)
            {
                
                totalCanjes += item.cantMonedasCambiadas + item.saldoAnterior;
                canje += item.cantMonedasCambiadas;
                ViewBag.TotalC = totalCanjes;
                ViewBag.Canje = canje;
            }


            return View(v);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
