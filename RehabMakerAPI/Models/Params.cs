namespace RehabMakerAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Params
    {
        [Key]
        public int IdParams { get; set; }

        public decimal Speed { get; set; }

        public decimal Distance { get; set; }

        public decimal Сalories { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }

        public int IdDevice { get; set; }

        public virtual Device Device { get; set; }
    }
}
