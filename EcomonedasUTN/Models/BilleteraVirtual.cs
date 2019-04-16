namespace EcomonedasUTN.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BilleteraVirtual")]
    public partial class BilleteraVirtual
    {
        public int id { get; set; }

        public double total { get; set; }

        [Required]
        [StringLength(50)]
        public string idUsuario { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
