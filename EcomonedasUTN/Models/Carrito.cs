using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcomonedasUTN.Models
{
    public class Carrito
    {
        public List<CarritoCompras> Items { get; private set; }

        //Implementación Singleton
        //Las propiedades de solo lectura solo se pueden establecer en la inicialización o en un constructor
        public static readonly Carrito Instancia;

        //Se llama al constructor estatico tan pronto como la clase se carga en la memoria
        static Carrito()
        {
            //Si el carrito no esta en la sesión, cree uno y guarde los items.
            if (HttpContext.Current.Session["carrito"] == null)
            {
                Instancia = new Carrito();
                Instancia.Items = new List<CarritoCompras>();
                HttpContext.Current.Session["carrito"] = Instancia;
            }
            else
            {
                // De lo contrario, obtengalo de la sesión.
                Instancia = (Carrito)HttpContext.Current.Session["carrito"];
            }
        }

        //Un constructor protegido asegura que un objeto no se puede crear desde el exterior
        protected Carrito() { }

        
        //AgregarItem (): agrega un material a la orden
        public void AgregarItem(int elementoID)
        {
            // Crear un nuevo material para agregar al carrito
            CarritoCompras nuevoItem = new CarritoCompras(elementoID);
            // Si este material ya existe en lista de materiales, aumente la cantidad
            // De lo contrario, agregue el nuevo elemento a la lista

            if (Items.Exists(x => x.elementoID == elementoID))
            {
                CarritoCompras item = Items.Find(x => x.elementoID == elementoID);
                item.Cantidad++;
                return;
            }
            nuevoItem.Cantidad = 1;
            Items.Add(nuevoItem);
        }

        //SetItemCantidad(): cambia la cantidad de un material en el carrito
        public void SetItemCantidad(int elementoID, int cantidad)
        {
            // Si estamos configurando la cantidad a 0, elimine el material por completo
            if (cantidad == 0)
            {
                EliminarItem(elementoID);
                return;
            }

            // Encuentra el material y actualiza la cantidad
            CarritoCompras actualizarItem = new CarritoCompras(elementoID);
            if (Items.Exists(x => x.elementoID == elementoID))
            {
                CarritoCompras item = Items.Find(x => x.elementoID == elementoID);
                item.Cantidad = cantidad;
                return;
            }
        }

        //EliminarItem (): elimina un Material del carrito de compras
        public void EliminarItem(int elementoID)
        {
            if (Items.Exists(x => x.elementoID == elementoID))
            {
                var itemEliminar = Items.Single(x => x.elementoID == elementoID);
                Items.Remove(itemEliminar);
            }
        }

        public void limpiarCarrito()
        {
            Instancia.Items.Clear();
        }

        //GetSubTotal() - Devuelve el precio total de todos los materiales.
        public decimal GetTotal()
        {
            decimal subTotal = 0;
            foreach (CarritoCompras item in Items)
                subTotal += item.subTotal();

            return subTotal;
        }

        public int cantElementos()
        {
            int cant = 0;
            foreach (CarritoCompras item in Items)
            {
                cant += item.Cantidad;
            }
            return cant;
        }
    }
}