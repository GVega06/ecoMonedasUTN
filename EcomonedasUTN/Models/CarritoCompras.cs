using System;
using System.Linq;

namespace EcomonedasUTN.Models
{
    public class CarritoCompras
    {
        public int elementoID { get; set; }

        public int Cantidad { get; set; }

        //Para establecer el precio del producto
        public double? precioUnitario
        {
            get { return Material.precio; }
        }
        public virtual Material Material
        {
            get; set;
        }
        public decimal subTotal()
        {
            return Convert.ToDecimal(precioUnitario * Cantidad);
        }
        public CarritoCompras()
        {

        }
        public CarritoCompras(int elementoID)
        {
            this.elementoID = elementoID;
            this.Material = new Material();
            Material = obtenerElemento(elementoID);
        }

        public Material obtenerElemento(int id)
        {
            var db = new ecoMonedaModel();
            Material miElemento = db.Material.Where(x => x.id == id).First<Material>();
            return miElemento;
        }
    }
}