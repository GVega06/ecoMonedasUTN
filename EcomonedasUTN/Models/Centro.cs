namespace EcomonedasUTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Centro")]
    public partial class Centro
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Centro()
        {
            EncCambio = new HashSet<EncCambio>();
        }

        public int id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Nombre requerido")]    
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Provincia requerida")]
        [Display(Name = "Provincia")]
        public string provincia { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Dirección requerida")]
        [Display(Name = "Dirección")]
        [DataType(DataType.MultilineText)]
        public string direccion { get; set; }

        [Display(Name = "Activo")]
        [UIHint("SiNo")]
        [Required(ErrorMessage = "Estado requerido")]
        public bool? estado { get; set; }

        [StringLength(50)]
        [Display(Name = "Usuario")]
        public string idUsuario { get; set; }

        [Display(Name = "Usuario")]
        public virtual Usuario Usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EncCambio> EncCambio { get; set; }
    }
}
