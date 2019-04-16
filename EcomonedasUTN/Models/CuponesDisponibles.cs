namespace EcomonedasUTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CuponesDisponibles
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string idUsuario { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idCupon { get; set; }

        public bool usado { get; set; }

        public DateTime fechaAdquirido { get; set; }

        public long idUnico { get; set; }

        public virtual Cupon Cupon { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
