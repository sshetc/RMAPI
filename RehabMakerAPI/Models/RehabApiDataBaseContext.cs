namespace RehabMakerAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RehabApiDataBaseContext : DbContext
    {
        public RehabApiDataBaseContext()
            : base("name=RehabApiDataBaseContext")
        {
        }

        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<Params> Params { get; set; }
        public virtual DbSet<Statistics> Statistics { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>()
                .HasMany(e => e.Params)
                .WithRequired(e => e.Device)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Device>()
                .HasMany(e => e.Statistics)
                .WithRequired(e => e.Device)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Params>()
                .Property(e => e.Speed)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Params>()
                .Property(e => e.Distance)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Params>()
                .Property(e => e.Сalories)
                .HasPrecision(5, 2);
        }
    }
}
