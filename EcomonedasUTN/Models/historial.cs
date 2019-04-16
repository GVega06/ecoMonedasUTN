namespace EcomonedasUTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("historial")]
    public partial class historial
    {
        public int id { get; set; }

        public double cantMonedasCambiadas { get; set; }

        public double saldoAnterior { get; set; }

        public DateTime fecha { get; set; }

        public int idCupon { get; set; }

        [Required]
        [StringLength(50)]
        public string idUsuario { get; set; }

        public virtual Cupon Cupon { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
