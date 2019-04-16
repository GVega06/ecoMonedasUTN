using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EcomonedasUTN.Models;
using System.IO;
using System.Drawing;
using System.Web.Helpers;

namespace EcomonedasUTN.Controllers
{
    public class MaterialController : Controller
    {
        private ecoMonedaModel db = new ecoMonedaModel();

        // GET: Material
        public ActionResult Index()
        {
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.Mensaje = TempData["mensaje"].ToString();
            }
            return View(db.Material.ToList());
        }

        // GET: Material/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //Respuesta al usuario
                TempData["mensaje"] = "Especifique un material reciclable";
                return RedirectToAction("Index");
            }
            Material material = db.Material.Find(id);
            if (material == null)
            {
                TempData["mensaje"] = "No existe el material reciclable";
                return RedirectToAction("Index");
            }
           
            return View(material);
        }

        // GET: Material/Create
        public ActionResult Create()
        {
            ViewBag.Colores = llenarColores();
            return View();
        }

        public SelectList llenarColores()
        {
            var lista = new List<object>();
            lista.Add(Color.Blue);
            lista.Add(Color.Yellow);

            return new SelectList(lista);

        }

        // POST: Material/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,precio,color,imagen")] Material material)
        {
            HttpPostedFileBase FileBase = Request.Files[0];

            WebImage image = new WebImage(FileBase.InputStream);

            material.imagen = image.GetBytes();
            if (ModelState.IsValid)
            {
                db.Material.Add(material);
                db.SaveChanges();
                TempData["mensaje"] = "Material reciclable guardado con éxito";
                return RedirectToAction("Index");
            }
            TempData["mensaje"] = "No se pudo guardar el material reciclable";
            return View(material);
        }

        // GET: Material/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //Respuesta al usuario
                TempData["mensaje"] = "Especifique un material reciclable";
                return RedirectToAction("Index");
            }
            Material material = db.Material.Find(id);
            if (material == null)
            {
                TempData["mensaje"] = "No existe el material reciclable";
                return RedirectToAction("Index");
            }
            return View(material);
        }

        // POST: Material/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,precio,color,imagen")] Material material)
        {
            byte[] imageActual = null;

            HttpPostedFileBase FileBase = Request.Files[0];

            if (FileBase == null)
            {
                imageActual = db.Material.SingleOrDefault(t => t.id == material.id).imagen;
            }
            else
            {
                WebImage image = new WebImage(FileBase.InputStream);
                material.imagen = image.GetBytes();
            }

            if (ModelState.IsValid)
            {
                db.Entry(material).State = EntityState.Modified;
                db.SaveChanges();
                TempData["mensaje"] = "Material reciclable modificado con éxito";
                return RedirectToAction("Index");
            }
            TempData["mensaje"] = "No se pudo modificar el material reciclable";
            return View(material);
        }

        // GET: Material/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //Respuesta al usuario
                TempData["mensaje"] = "Especifique un material reciclable";
                return RedirectToAction("Index");
            }
            Material material = db.Material.Find(id);
            if (material == null)
            {
                TempData["mensaje"] = "No existe el material reciclable";
                return RedirectToAction("Index");
            }
            return View(material);
        }

        // POST: Material/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Material material = db.Material.Find(id);
            db.Material.Remove(material);
            db.SaveChanges();
            TempData["mensaje"] = "Material reciclable eliminado con éxito";
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

        public ActionResult getImage(int id)
        {
            Material material = db.Material.Find(id);
            byte[] byteImage = material.imagen;

            System.IO.MemoryStream memory = new MemoryStream(byteImage);
            Image image = Image.FromStream(memory);

            memory = new MemoryStream();
            image.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg);
            memory.Position = 0;
            return File(memory, "image/jpg");
        }
    }
}
