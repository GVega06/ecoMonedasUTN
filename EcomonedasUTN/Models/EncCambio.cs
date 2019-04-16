namespace EcomonedasUTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EncCambio")]
    public partial class EncCambio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EncCambio()
        {
            DetalleCambio = new HashSet<DetalleCambio>();
        }

        public int id { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime fecha { get; set; }

        [Required]
        [StringLength(50)]
        public string idUsuario { get; set; }

        public int idCentro { get; set; }

        public double total { get; set; }

        public virtual Centro Centro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleCambio> DetalleCambio { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
