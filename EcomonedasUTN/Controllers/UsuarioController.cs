using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EcomonedasUTN.Models;
using System.Web.Security;

namespace EcomonedasUTN.Controllers
{
    public class UsuarioController : Controller
    {
        enum AllViewsNames
        {
            RazorIndex,
            ASPXIndex
        }

        static AllViewsNames currentViewEnum = AllViewsNames.ASPXIndex;
        private ecoMonedaModel db = new ecoMonedaModel();
        string strCurrentView = currentViewEnum == AllViewsNames.RazorIndex ? "Index" : "Inicio";

        // GET: Usuario
        public ActionResult Index()
        {
            var usuario = db.Usuario.Include(u => u.Rol);
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.Mensaje = TempData["mensaje"].ToString();
            }
            return View(usuario.ToList());

        }

        // GET: Usuario/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            if (TempData.ContainsKey("mensajes"))
            {
                ViewBag.Mensaje1 = TempData["mensajes"].ToString();
            }

            ViewBag.Provincia = cargarProvinciasDropDownList();
            ViewBag.idRol = new SelectList(db.Rol, "idRol", "descripcion");
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "email,nombre,clave,telefono,direccion")] Usuario usuario, string provincia, string repetirClave)
        {

            usuario.direccion = provincia + " " + usuario.direccion;
            usuario.idRol = 3;
            usuario.estado = true;
            if (ModelState.IsValid)
            {
                if (repetirClave.Equals(usuario.clave)) {

                    if (!consulta(usuario.email))
                    {

                        db.Usuario.Add(usuario);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["mensajes"] = "Usuario se encuentra registrado";
                        repetirClave = "";
                    }

                }

                else
                {
                    TempData["mensajes"] = "Las contraseñas no coinciden";
                    repetirClave = "";
                }
            }

            if (TempData.ContainsKey("mensajes"))
            {
                ViewBag.Mensaje1 = TempData["mensajes"].ToString();
            }
            ViewBag.Provincia = cargarProvinciasDropDownList();
            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.Provincia = cargarProvinciasDropDownList();
            ViewBag.idRol = new SelectList(db.Rol, "idRol", "descripcion", usuario.idRol);
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "email,nombre,clave,telefono,direccion")] Usuario usuario)
        {

            usuario.idRol = usuario.idRol;
            usuario.estado = usuario.estado;
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idRol = new SelectList(db.Rol, "idRol", "descripcion", usuario.idRol);
            return View(usuario);
        }

        //KeylorSk8
        // GET: Usuario/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Usuario usuario = db.Usuario.Find(id);
            db.Usuario.Remove(usuario);
            db.SaveChanges();
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

        public bool consulta(string id)
        {
            Models.Usuario usuario = db.Usuario.Find(id);


            if (usuario.email.Equals(id))
            {
                return true;
            }

            return false;
        }




        public ActionResult InicioSesion()
        {
      

            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult InicioSesion(string email, string clave)
        {


            
                Models.Usuario user = db.Usuario.Find(email);

    
            if (user.email.Equals(email) && user.clave.Equals(clave) && user.estado == true)
            {
                System.Web.HttpContext.Current.Session["email"] = email;
                System.Web.HttpContext.Current.Session["nombre"] = user.nombre;
                System.Web.HttpContext.Current.Session["clave"] = clave;
                System.Web.HttpContext.Current.Session["idRol"] = user.idRol;
                System.Web.HttpContext.Current.Session["estado"] = user.estado;
                System.Web.HttpContext.Current.Session["telefono"] = user.telefono;
                System.Web.HttpContext.Current.Session["direccion"] = user.direccion;
                return RedirectToAction("Index", "Inicio");

            }


            if (TempData.ContainsKey("mensajeUser"))
            {
                ViewBag.MensajeUser = TempData["mensajeUser"].ToString();
            }


            TempData["mensajeUser"] = "Usuario o contraseña incorrecta";
                    return View();

               


           

            
        }


        private void LoadSessionObject()
        {
            // Load session from HttpContext.
            ViewData["email"] = System.Web.HttpContext.Current.Session["email"] as String;
            ViewData["nombre"] = System.Web.HttpContext.Current.Session["nombre"] as String;
            ViewData["clave"] = System.Web.HttpContext.Current.Session["clave"] as String;
            ViewData["idRol"] = System.Web.HttpContext.Current.Session["idRol"] as String;
            ViewData["estado"] = System.Web.HttpContext.Current.Session["estado"] as String;
            ViewData["telefono"] = System.Web.HttpContext.Current.Session["telefono"] as String;
            ViewData["direccion"] = System.Web.HttpContext.Current.Session["direccion"] as String;
        }
    }
}
