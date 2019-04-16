namespace EcomonedasUTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DetalleCambio")]
    public partial class DetalleCambio
    {
        public int id { get; set; }

        public int idEncCambio { get; set; }

        public int idMaterial { get; set; }

        public int cantidad { get; set; }

        public double subtotal { get; set; }

        public virtual EncCambio EncCambio { get; set; }

        public virtual Material Material { get; set; }
    }
}
