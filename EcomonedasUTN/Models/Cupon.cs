namespace EcomonedasUTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cupon")]
    public partial class Cupon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cupon()
        {
            CuponesDisponibles = new HashSet<CuponesDisponibles>();
            historial = new HashSet<historial>();
        }

        public int id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Nombre requerido")]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }


        [StringLength(250)]
        [Required(ErrorMessage = "Descripción requerida")]
        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "Valor requerido")]
        [Display(Name = "Valor")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^[0-9]*(\,)?[0-9]?[0-9]?$", ErrorMessage ="Sólo números"),]
        public double valor { get; set; }

        [Display(Name = "Activo")]
        [UIHint("SiNo")]
        [Required(ErrorMessage = "Estado requerido")]
        public bool? estado { get; set; }

        [Column(TypeName = "image")]
        [Display(Name = "Imagen")]
      
        public byte[] imagen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CuponesDisponibles> CuponesDisponibles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<historial> historial { get; set; }
    }
}
