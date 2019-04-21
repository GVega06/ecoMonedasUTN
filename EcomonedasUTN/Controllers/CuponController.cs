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


        public ActionResult Cupon()
        {
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




            var query = from r in db.CuponesDisponibles
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

  
    }

  
}
