﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EcomonedasUTN.Models;
using System.Globalization;
using System.Web.UI.WebControls;

namespace EcomonedasUTN.Controllers
{
    public class CentroController : Controller
    {
        private ecoMonedaModel db = new ecoMonedaModel();

        // GET: Centro
        public ActionResult Index()
        {
            ViewBag.Provincia = cargarProvinciasDropDownList();
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
            ViewBag.Usuario = cargarUsuarioDropDownList();
            if (TempData.ContainsKey("mensajeCentro"))
            {
                ViewBag.MensajeCentro = TempData["mensajeCentro"].ToString();
            }
            return View();
        }

        private SelectList cargarUsuarioDropDownList(object selected = null)
        {
            var listado = db.Usuario.OrderBy(x => x.nombre).Where(x => x.idRol ==2);

            return new SelectList(listado, "email", "nombre", selected);
        }

        private bool consultaCentro(string email)
        {

            var lista= db.Centro.Where(x => x.idUsuario.Equals(email));
            return true;
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
                if (consultaCentro(centro.idUsuario))
                {
                    TempData["mensajeCentro"] = "Este usuario ya tiene asignado un centro";
                    return View(centro);
                }

                db.Centro.Add(centro);
                db.SaveChanges();
                TempData["mensaje"] = "Centro de Acopio guardado con éxito";
                return RedirectToAction("Index");
            }
            ViewBag.Provincia = cargarProvinciasDropDownList();
            ViewBag.Usuario = cargarUsuarioDropDownList();
            TempData["mensaje"] = "No se pudo guadar el centro de acopio";
            if (TempData.ContainsKey("mensajeCentro"))
            {
                ViewBag.MensajeCentro = TempData["mensajeCentro"].ToString();
            }
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
            ViewBag.Usuario = cargarUsuarioDropDownList();
            return View(centro);
        }

        // POST: Centro/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,provincia,direccion,estado,idUsuario")] Centro centro)
        {

            if (consultaCentro(centro.idUsuario))
            {
                TempData["mensaje"] = "Este usuario ya tiene asignado un centro";
                return View(centro);
            }

            if (ModelState.IsValid)
            {
                db.Entry(centro).State = EntityState.Modified;
                db.SaveChanges();
                TempData["mensaje"] = "Centro de Acopio modificado con éxito";
                return RedirectToAction("Index");
            }
            TempData["mensaje"] = "No se pudo modificar el centro de acopio";
            ViewBag.Usuario = cargarUsuarioDropDownList();
            ViewBag.Provincia = cargarProvinciasDropDownList();
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
           var lst = new List<SelectListItem>();    
            //De la siguiente manera llenamos manualmente,
            //Siendo el campo Text lo que ve el usuario y
            //el campo Value lo que en realidad vale nuestro valor
            lst.Add(new SelectListItem() { Text = "San José", Value = "San José" });
            lst.Add(new SelectListItem() { Text = "Alajuela", Value = "Alajuela" });
            lst.Add(new SelectListItem() { Text = "Cartago", Value = "Cartago" });
            lst.Add(new SelectListItem() { Text = "Heredia", Value = "Heredia" });
            lst.Add(new SelectListItem() { Text = "Guanacaste", Value = "Guanacaste" });
            lst.Add(new SelectListItem() { Text = "Puntarenas", Value = "Puntarenas" });
            lst.Add(new SelectListItem() { Text = "Limón", Value = "Limón" });
  
       
            return new SelectList(lst, "Value", "Text", selected);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ListaCentros()
        {
            return View(db.Centro.ToList());
        }

        public ActionResult filtrarCentrosAjax(string terminoBusqueda)
        {
            if (terminoBusqueda != null)
            {
                var lista = db.Centro.Where(x => x.nombre.Contains(terminoBusqueda) || x.direccion.Contains(terminoBusqueda) && x.estado.Equals("true")) ;
                return PartialView("_ListaCentros", lista.ToList());
            }
            return View();
        }
    }
}
