namespace EcomonedasUTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Usuario")]
    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            BilleteraVirtual = new HashSet<BilleteraVirtual>();
            Centro = new HashSet<Centro>();
            CuponesDisponibles = new HashSet<CuponesDisponibles>();
            EncCambio = new HashSet<EncCambio>();
            historial = new HashSet<historial>();
        }

        [Key]
        [StringLength(50)]

        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "Correo Electrónico requerido")]
        [RegularExpression(@"^([0-9a-zA-Z]([_.w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-w]*[0-9a-zA-Z].)+([a-zA-Z]{2,9}.)+[a-zA-Z]{2,3})$")]
        public string email { get; set; }

        [Required(ErrorMessage = "Nombre requerido")]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Contraseña requerida")]
        [StringLength(50)]
        [Display(Name = "Contraseña")]
        public string clave { get; set; }
 

        [Display(Name = "Rol")]
        public int idRol { get; set; }

        [Display(Name = "Activo")]
        [UIHint("SiNo")]
        public bool estado { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "Teléfono requerido")]
        public int? telefono { get; set; }

        [StringLength(100)]
        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "Dirección requerida")]
        [DataType(DataType.MultilineText)]
        public string direccion { get; set; }
      
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BilleteraVirtual> BilleteraVirtual { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Centro> Centro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CuponesDisponibles> CuponesDisponibles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EncCambio> EncCambio { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<historial> historial { get; set; }

        [Display(Name = "Tipo Usuarios")]
        public virtual Rol Rol { get; set; }
    }
}
