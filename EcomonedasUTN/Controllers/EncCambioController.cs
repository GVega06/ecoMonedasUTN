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
            DateTime anterior = Convert.ToDateTime(fechaActual);
            DateTime actual = Convert.ToDateTime(fechaAnterior);
            if (TempData.ContainsKey("mensajeReporte"))
            {
                ViewBag.MensajeReporte = TempData["mensajeReporte"].ToString();
            }


            if (fechaAnterior != null && fechaActual != null)
            {
                if (anterior.Year> actual.Year)
                {
                    TempData["mensajeReporte"] = "Seleccione una fecha válida";
                }

                if(anterior.Year == actual.Year && anterior.Month > actual.Month)
                {
                    TempData["mensajeReporte"] = "Seleccione una fecha válida";
                }

                var query = from r in db.EncCambio
                            join c in db.Centro on r.idCentro equals c.id
                            where r.fecha == anterior
                        

                            select new
                            {
                                r.fecha,                          
                                c.nombre,
                                 total = r.total
                            };
                ViewBag.ReportViewer = Reporte.reporte(query.ToList(), "", "reporteEstadistica.rdlc");
                return PartialView("_estadisticaEcoMoneda", query.ToList());

            }
            return View();

        }
    }
}