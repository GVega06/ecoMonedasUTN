using EcomonedasUTN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcomonedasUTN.Controllers
{
    public class EncCambioController : Controller
    {
        private ecoMonedaModel db = new ecoMonedaModel();
        public ActionResult reporteEstadistica(string fechaAnterior, string fechaActual)
        {
        

            if (fechaAnterior != null && fechaActual != null)
            {
                DateTime anterior = Convert.ToDateTime(fechaAnterior);
                DateTime actual = Convert.ToDateTime(fechaActual);

                if (anterior.Year > actual.Year)
                {
                    TempData["mensajeReporte"] = "Seleccione una fecha válida";

                }
                else
                {
                    if (anterior.Year == actual.Year && anterior.Month > actual.Month)
                    {
                        TempData["mensajeReporte"] = "Seleccione una fecha válida";

                    }
                    else
                    {
                        if (anterior.Year == actual.Year && anterior.Month == actual.Month && anterior.Day > actual.Day)
                        {
                            TempData["mensajeReporte"] = "Seleccione una fecha válida";

                        }

                        else
                        {
                            if (actual > DateTime.Now)
                            {
                                TempData["mensajeReporte"] = "Seleccione una fecha válida";
                            }

                            else
                            {
                                var query = from r in db.EncCambio
                                            join c in db.Centro on r.idCentro equals c.id
                                            join u in db.Usuario on r.idUsuario equals u.email
                                            where r.fecha >= anterior && r.fecha <= actual


                                            select new
                                            {
                                                r.fecha,
                                                c.nombre,
                                                total = r.total,
                                                Usuario = u.nombre
                                            };


                                ViewBag.ReportViewer = Reporte.reporte(query.ToList(), "", "reporteEstadistica.rdlc");

                                return PartialView("_estadisticaEcoMoneda", query.ToList());


                            }
                        }
                    }
                }

                if (TempData.ContainsKey("mensajeReporte"))
                {
                    ViewBag.MensajeReporte = TempData["mensajeReporte"].ToString();
                    return PartialView("_mensaje");

                }
            }

                return View();

            }
    }
}