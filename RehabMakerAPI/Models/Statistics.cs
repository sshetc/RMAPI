namespace RehabMakerAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Statistics
    {
        [Key]
        public int IdStatitics { get; set; }

        [Required]
        public string AverageSpeed { get; set; }

        [Required]
        public string TotalDistance { get; set; }

        [Required]
        public string TotalCalories { get; set; }

        public DateTime Date { get; set; }

        public int IdDevice { get; set; }

        public virtual Device Device { get; set; }
    }
}
