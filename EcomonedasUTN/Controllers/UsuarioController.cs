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
            var usuario = db.Usuario.Where(u => u.idRol == 3);
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.Mensaje = TempData["mensaje"].ToString();
            }
            return View(usuario.ToList());

        }

        // GET: Usuario/Details/5
        public ActionResult Details()
        {
            Usuario user = ((Usuario)Session["session"]);

            return View(user);
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
                        Session["session"] = usuario;
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
        public ActionResult Edit()
        {
            Usuario user = ((Usuario)Session["session"]);
            if (user == null)
            {
                TempData["mensaje"] = "Usuario no existe";
                return RedirectToAction("Index");
            }
            //LoadSessionObject();
            ViewBag.Provincia = cargarProvinciasDropDownList();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string nombre, string telefono, string direccion, string provincia)
        {
            ViewBag.Provincia = cargarProvinciasDropDownList();
            //LoadSessionObject();

            Usuario user = ((Usuario)Session["session"]);

            ViewData["email"] = user.email;
            ViewData["idRol"] = user.idRol;
            ViewData["estado"] = user.estado;
            ViewData["clave"] = user.clave;
            user.telefono = Convert.ToInt32(telefono);
            user.nombre = nombre;
            user.direccion = provincia + " " + direccion;


            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                System.Web.HttpContext.Current.Session["email"] = user.email;
                System.Web.HttpContext.Current.Session["nombre"] = user.nombre;
                System.Web.HttpContext.Current.Session["clave"] = user.clave;
                System.Web.HttpContext.Current.Session["idRol"] = user.idRol;
                System.Web.HttpContext.Current.Session["estado"] = user.estado;
                System.Web.HttpContext.Current.Session["telefono"] = user.telefono;
                System.Web.HttpContext.Current.Session["direccion"] = user.direccion;
             
                Session["session"] = user;
                db.SaveChanges();
                return RedirectToAction("Details");
            }

            return View(user);
        }


        public ActionResult Clave()
        {
            Usuario user = ((Usuario)Session["session"]);
            if (user == null)
            {
                TempData["mensaje"] = "Usuario no existe";
                return RedirectToAction("Index");
            }
         
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Clave(string clave, string repetirClave)
        {
           
            //LoadSessionObject();

            Usuario user = ((Usuario)Session["session"]);

            ViewData["email"] = user.email;
            ViewData["idRol"] = user.idRol;
            ViewData["estado"] = user.estado;
             user.clave = clave;
            ViewData["telefono"] = user.telefono;
            ViewData["nombre"] = user.nombre;
            ViewData["direccion"] = user.direccion;


            if (clave.Equals(repetirClave)) { 

            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                    Session["session"] = null;
                    Session["session"] = user;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            }
            TempData["mensajeClave"] = "Contraseñas no coinciden";
            if (TempData.ContainsKey("mensajeClave"))
            {
                ViewBag.Mensaje = TempData["mensajeClave"].ToString();
            }
       
            return View(user);
        }

        public ActionResult EditarUser(string email)
        {
            if (email.Equals(""))
           {
               TempData["mensaje"] = "Especifique un usuario";
           }

            Usuario user = db.Usuario.Find(email);
            if (user == null)
            {
                TempData["mensaje"] = "Usuario no existe";
                return RedirectToAction("Index");
            }
            //LoadSessionObject();
            ViewBag.Rol = cargarRolDropDownList();
            return View(user);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUser([Bind(Include = "email,nombre,clave,idRol,estado,telefono,direccion")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Rol = cargarRolDropDownList();
            return View(usuario);
        }

        private dynamic cargarRolDropDownList(object selected = null)
        {
       
            var listado = db.Rol.OrderBy(x => x.descripcion);
            return new SelectList(listado, "idRol", "descripcion", selected);
    }

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

            Usuario user = db.Usuario.Find(email);
            if (user == null)
            {
                ViewBag.Mensaje = "Usuario no Existente";
                return RedirectToAction("InicioSesion");
            }



            if (user.email.Equals(email) && user.clave.Equals(clave))
            {

                if (user.estado == true)
                {


                    if (validaCentro(email)) { 
                    System.Web.HttpContext.Current.Session["email"] = email;
                    System.Web.HttpContext.Current.Session["nombre"] = user.nombre;
                    System.Web.HttpContext.Current.Session["clave"] = clave;
                    System.Web.HttpContext.Current.Session["idRol"] = user.idRol;
                    System.Web.HttpContext.Current.Session["estado"] = user.estado;
                    System.Web.HttpContext.Current.Session["telefono"] = user.telefono;
                    System.Web.HttpContext.Current.Session["direccion"] = user.direccion;
                    Session["session"] = user;
                    return RedirectToAction("Index", "Inicio");

                    }

                    else
                    {

                        TempData["mensajeUser"] = "Centro de Acopio se encuentra desahilitado, contáctese con el administrador";
                    }

                }
                else
                {
                    TempData["mensajeUser"] = "Usuario se encuentra inactivo";

                }
            }

            else
            {
                TempData["mensajeUser"] = "Usuario o contraseña incorrecta";
            }


            if (TempData.ContainsKey("mensajeUser"))
            {
                ViewBag.MensajeUser = TempData["mensajeUser"].ToString();
            }
            return View(user);         
        }


        public bool validaCentro(string email)
        {
            Usuario user = db.Usuario.Find(email);

            if (user.idRol == 2)
            {
                var centro = db.Centro.Where(x => x.idUsuario.Equals(email));

                foreach (var item in centro)
                {
                    if(item.estado == true)
                    {
                        return true;
                    }

                   if(item.estado == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void LoadSessionObject()
        {
            // Load session from HttpContext.
            ViewData["email"] = System.Web.HttpContext.Current.Session["email"] as String;
            ViewData["nombre"] = System.Web.HttpContext.Current.Session["nombre"] as String;
            ViewData["clave"] = System.Web.HttpContext.Current.Session["clave"] as String;
            ViewData["idRol"] = System.Web.HttpContext.Current.Session["idRol"] as String;
            ViewData["estado"] = System.Web.HttpContext.Current.Session["estado"] as String;
            ViewData["telefono"] = System.Web.HttpContext.Current.Session["telefono"];
            ViewData["direccion"] = System.Web.HttpContext.Current.Session["direccion"] as String;
        }
      
        public ActionResult CerrarSession()
        {
            Session["session"] = null;
            return Redirect("InicioSesion");
        }

    }
}
