using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EcomonedasUTN.Models;
using System.Globalization;

namespace EcomonedasUTN.Controllers
{
    public class CentroController : Controller
    {
        private ecoMonedaModel db = new ecoMonedaModel();

        // GET: Centro
        public ActionResult Index()
        {

            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.Mensaje = TempData["mensaje"].ToString();
            }
            return View(db.Centro.ToList());
        }

        // GET: Centro/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                //Respuesta al usuario
                TempData["mensaje"] = "Especifique un centro de acopio";
                return RedirectToAction("Index");
            }
            Centro centro = db.Centro.Find(id);

            if (centro == null)
            {
                TempData["mensaje"] = "No existe el centro de acopio";
                return RedirectToAction("Index");
            }

            return View(centro);
        }

        // GET: Centro/Create
        public ActionResult Create()
        {

            ViewBag.Provincia = cargarProvinciasDropDownList();
            ViewBag.idUsuario = new SelectList(db.Usuario, "email", "nombre");
            return View();
        }

        // POST: Centro/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,provincia,direccion,estado,idUsuario")] Centro centro)
        {
            if (ModelState.IsValid)
            {
                db.Centro.Add(centro);
                db.SaveChanges();
                TempData["mensaje"] = "Centro de Acopio guardado con éxito";
                return RedirectToAction("Index");
            }
            ViewBag.Provincia = cargarProvinciasDropDownList();
            ViewBag.idUsuario = new SelectList(db.Usuario, "email", "nombre", centro.idUsuario);
            TempData["mensaje"] = "No se pudo guadar el centro de acopio";
            return View(centro);
        }

        // GET: Centro/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                //Respuesta al usuario
                TempData["mensaje"] = "Especifique un centro de acopio";
                return RedirectToAction("Index");
            }
            Centro centro = db.Centro.Find(id);

            if (centro == null)
            {
                TempData["mensaje"] = "No existe el centro de acopio";
                return RedirectToAction("Index");
            }

            ViewBag.Provincia = cargarProvinciasDropDownList();
            ViewBag.idUsuario = new SelectList(db.Usuario, "email", "nombre", centro.idUsuario);
            return View(centro);
        }

        // POST: Centro/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,provincia,direccion,estado,idUsuario")] Centro centro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(centro).State = EntityState.Modified;
                db.SaveChanges();
                TempData["mensaje"] = "Centro de Acopio modificado con éxito";
                return RedirectToAction("Index");
            }
            TempData["mensaje"] = "No se pudo modificar el centro de acopio";
            ViewBag.idUsuario = new SelectList(db.Usuario, "email", "nombre", centro.idUsuario);
            return View(centro);
        }

        // GET: Centro/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Respuesta al usuario
                TempData["mensaje"] = "Especifique un centro de acopio";
                return RedirectToAction("Index");
            }
            Centro centro = db.Centro.Find(id);

            if (centro == null)
            {
                TempData["mensaje"] = "No existe el centro de acopio";
                return RedirectToAction("Index");
            }
            return View(centro);
        }

        // POST: Centro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Centro centro = db.Centro.Find(id);
            db.Centro.Remove(centro);
            db.SaveChanges();
            TempData["mensaje"] = "Centro de Acopio eliminado con éxito";
            return RedirectToAction("Index");
        }

        private SelectList cargarProvinciasDropDownList(object selected = null)
        {
            var listaProvincia = new List<object>();
            listaProvincia.Add("San José");
            listaProvincia.Add("Alajuela");
            listaProvincia.Add("Cartago");
            listaProvincia.Add("Heredia");
            listaProvincia.Add("Guanacaste");
            listaProvincia.Add("Puntarenas");
            listaProvincia.Add("Limón");
            return new SelectList(listaProvincia, selected);
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
