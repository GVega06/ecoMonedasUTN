using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EcomonedasUTN.Models;
using System.Web.Helpers;
using System.IO;
using System.Drawing;
using System.Text;

namespace EcomonedasUTN.Controllers
{
    public class CuponController : Controller
    {
        private ecoMonedaModel db = new ecoMonedaModel();
       
        // GET: Cupon
        public ActionResult Index()
        {
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.Mensaje = TempData["mensaje"].ToString();
            }
            return View(db.Cupon.ToList());
        }

        // GET: Cupon/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                //Respuesta al usuario
                TempData["mensaje"] = "Especifique un cupón";
                return RedirectToAction("Index");
            }
            Cupon cupon = db.Cupon.Find(id);
            if (cupon == null)
            {
                TempData["mensaje"] = "No existe el cupón";
                return RedirectToAction("Index");
            }

           
            return View(cupon);
        }

        // GET: Cupon/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cupon/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,descripcion,valor,estado,imagen")] Cupon cupon)
        {
            HttpPostedFileBase FileBase = Request.Files[0];
            WebImage image = new WebImage(FileBase.InputStream);

            cupon.imagen = image.GetBytes();
            if (ModelState.IsValid)
            {

                db.Cupon.Add(cupon);
                db.SaveChanges();
                TempData["mensaje"] = "Cupón guardado con éxito";
                return RedirectToAction("Index");
            }
            TempData["mensaje"] = "No se pudo guardar el cupón";
            return View(cupon);
        }

        // GET: Cupon/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //Respuesta al usuario
                TempData["mensaje"] = "Especifique un cupón";
                return RedirectToAction("Index");
            }
            Cupon cupon = db.Cupon.Find(id);
            if (cupon == null)
            {
                TempData["mensaje"] = "No existe el cupón";
                return RedirectToAction("Index");
            }
            return View(cupon);
        }

        // POST: Cupon/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,descripcion,valor,estado,imagen")] Cupon cupon)
        {
            byte[] imageActual = null;

            HttpPostedFileBase FileBase = Request.Files[0];
            
            if(FileBase == null)
            {
                imageActual = db.Cupon.SingleOrDefault(t => t.id == cupon.id).imagen;
            }else
            {
                WebImage image = new WebImage(FileBase.InputStream);
                cupon.imagen = image.GetBytes();
            }

         
            if (ModelState.IsValid)
            {
                db.Entry(cupon).State = EntityState.Modified;
                db.SaveChanges();
                TempData["mensaje"] = "Cupón modificado con éxito";
                return RedirectToAction("Index");
            }
            TempData["mensaje"] = "No se pudo actualizar el cupón";
            return View(cupon);
        }

        // GET: Cupon/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Respuesta al usuario
                TempData["mensaje"] = "Especifique un cupón";
                return RedirectToAction("Index");
            }
            Cupon cupon = db.Cupon.Find(id);
            if (cupon == null)
            {
                TempData["mensaje"] = "No existe el cupón";
                return RedirectToAction("Index");
            }
            return View(cupon);
        }

        // POST: Cupon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cupon cupon = db.Cupon.Find(id);
            db.Cupon.Remove(cupon);
            db.SaveChanges();
            TempData["mensaje"] = "Cupón eliminado con éxito";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult Cupon(int id)
        {

            Usuario user = ((Usuario)Session["session"]);
            BilleteraVirtual billetera = db.BilleteraVirtual.Where(x => x.idUsuario.Equals(user.email)).First();

            historial historia = new historial();
            CuponesDisponibles cuponD = new CuponesDisponibles();
            Cupon cupon = db.Cupon.Find(id);


            if (cupon.valor <= billetera.total)
            {
                historia.saldoAnterior = billetera.total;
                historia.cantMonedasCambiadas += cupon.valor;
                historia.saldoAnterior = billetera.total;
                historia.fecha = DateTime.Now;
                historia.idUsuario = user.email;
                historia.idCupon = cupon.id;
                cuponD.idUsuario = user.email;
                cuponD.idCupon = cupon.id;
                cuponD.usado = true;
                cuponD.fechaAdquirido = historia.fecha;
                cuponD.idUnico = cupon.id;
                billetera.total -= cupon.valor;
                cupon.estado = false;
                db.CuponesDisponibles.Add(cuponD);
                db.historial.Add(historia);
                db.Entry(billetera).State = EntityState.Modified;
                db.Entry(cupon).State = EntityState.Modified;
                db.SaveChanges();
            }


            else
            {
                TempData["mensajeCanje"] = "No tiene eco monedas suficientes";
                return RedirectToAction("ListaCupones");
            }

            Random ran = new Random();
            string posibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890123456789";
            int longitud = posibles.Length;
            char letra;
            int longitudnuevacadena = 16;
            string nuevacadena = "";
            for (int i = 0; i < longitudnuevacadena; i++)
            {
                letra = posibles[ran.Next(longitud)];
                nuevacadena += letra.ToString();
            }

            var query = from r in db.CuponesDisponibles.Where(x => x.idCupon == id)
                        join t in db.Usuario on r.idUsuario equals t.email
                        join c in db.Cupon on r.idCupon equals c.id
                        select new
                        {
                            c.nombre,
                            r.fechaAdquirido,
                            c.valor,
                            Usuario = t.nombre,
                            codigo = nuevacadena

                        };


            
            ViewBag.ReportViewer = Reporte.reporte(query.ToList(), "", "CuponCanje.rdlc");
            return View();
        }
        
        public ActionResult ListaCupones()
        {
           if (TempData.ContainsKey("mensajeCanje"))
            {
                ViewBag.MensajeCanje = TempData["mensajeCanje"].ToString();
            }
            return View(db.Cupon.Where(x => x.estado == true).ToList());
        }

        public ActionResult filtrarCuponesAjax(string terminoBusqueda)


        {
            if (terminoBusqueda != null)
            {
                var lista = db.Cupon.Where(x => x.nombre.Contains(terminoBusqueda) || x.descripcion.Contains(terminoBusqueda));
                return PartialView("_ListaCupon", lista.ToList());
            }
            return View();
        }

        public ActionResult reporteHistorial()
        {
            Usuario user = ((Usuario)Session["session"]);

            var query = from r in db.historial.Where(x => x.idUsuario.Equals(user.email))
                        join t in db.Usuario on r.idUsuario equals t.email
                        join c in db.Cupon on r.idCupon equals c.id
                        select new
                        {
                            c.nombre,
                            r.fecha,
                            r.cantMonedasCambiadas,
                            r.saldoAnterior,
                            c.valor,
                            usuario = t.nombre,
                           
                        };



            ViewBag.ReportViewer = Reporte.reporte(query.ToList(), "", "reporteHistorial.rdlc");
            return View();
        }

        public ActionResult reporteEcoMonedasGeneradas()
        {
            Usuario user = ((Usuario)Session["session"]);

            var query = from r in db.historial.Where(x => x.idUsuario.Equals(user.email))
                        
                        select new
                        {                       
                            r.cantMonedasCambiadas,
                            r.saldoAnterior,
                            Total = r.cantMonedasCambiadas + r.saldoAnterior                        

                        };



            ViewBag.ReportViewer = Reporte.reporte(query.ToList(), "", "reporteEcomonedasGeneradas.rdlc");
            return View();
        }
    }
    
}
