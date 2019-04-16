namespace EcomonedasUTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Material")]
    public partial class Material
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material()
        {
            DetalleCambio = new HashSet<DetalleCambio>();
        }

        public int id { get; set; }

  
        [StringLength(50)]
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Nombre requerido")]
        public string nombre { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "Precio requerido")]
        public double precio { get; set; }

    
        [StringLength(50)]
        [Display(Name = "Color")]
        [Required(ErrorMessage = "Color requerido")]
        public string color { get; set; }

        [Column(TypeName = "image")]
        [Display(Name = "Imagen")]
        public byte[] imagen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleCambio> DetalleCambio { get; set; }
    }
}
