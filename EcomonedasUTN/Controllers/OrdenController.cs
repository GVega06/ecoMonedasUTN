using EcomonedasUTN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcomonedasUTN.Controllers
{
    public class OrdenController : Controller
    {
        // GET: Orden
        public ActionResult Index()
        {
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.Mensaje = TempData["mensaje"].ToString();
            }
            return View();
        }

        public ActionResult AsignarCliente(string correo)
        {
            var db = new ecoMonedaModel();
            Usuario user = db.Usuario.Find(correo);
            if (user.idRol == 3)
            {
                Session["cliente"] = user.email;
            }else
            {
                Session["cliente"] = null;
            }
            
            return PartialView("_Cliente");
        }

        public ActionResult EliminarCliente()
        {
            Session["cliente"] = null;
            return View("Index");
        }

        public ActionResult ProcesarOrden()
        {
            var db = new ecoMonedaModel();
            Usuario user = ((Usuario)Session["session"]);
            int i = 0;

            if(Session["cliente"] == null)
            {
                TempData["mensaje"] = "Digite el correo del cliente";
                return RedirectToAction("Index");
            }

           foreach(var item in Carrito.Instancia.Items)
            {
                i++;
            }
           if(i > 0)
            {
                EncCambio encabezado = new EncCambio();
                var lista = db.Centro;

                foreach (var item in lista)
                {
                    if(item.Usuario.email == user.email)
                    {
                        encabezado.idCentro = item.id;
                    }
                }

                encabezado.idUsuario = Session["cliente"].ToString();
                encabezado.fecha = DateTime.Now;
                encabezado.total = Convert.ToDouble(Carrito.Instancia.GetTotal());
                db.EncCambio.Add(encabezado);
                db.SaveChanges();

                foreach (var item in Carrito.Instancia.Items)
                {
                    DetalleCambio detalle = new DetalleCambio();
                    detalle.idEncCambio = encabezado.id;
                    detalle.cantidad = item.Cantidad;
                    detalle.idMaterial = item.Material.id;
                    detalle.subtotal = (item.Material.precio * item.Cantidad);
                    db.DetalleCambio.Add(detalle);
                    db.SaveChanges();
                }

                Usuario usuario = db.Usuario.Find(Session["cliente"].ToString());
                BilleteraVirtual v = db.BilleteraVirtual.Where(x => x.idUsuario == usuario.email).First();
                v.total += encabezado.total;

                db.BilleteraVirtual.Add(v);
                db.SaveChanges();
                Carrito.Instancia.limpiarCarrito();
                TempData["mensaje"] = "Orden procesada correctamente";
                return RedirectToAction("Index", "Orden");
            }else
            {
                TempData["mensaje"] = "Agrega Materiales a la orden para continuar";
                return RedirectToAction("Index");
            }
        }
    }
}