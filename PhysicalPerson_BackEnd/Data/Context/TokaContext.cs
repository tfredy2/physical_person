using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PhysicalPerson.Configurations;
using PhysicalPerson.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhysicalPerson.Data.Context
{
    public partial class TokaContext: DbContext
    {
        public TokaContext()
        {
        }

        public TokaContext(DbContextOptions<TokaContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ServiceExtensions.Configuracion.GetConnectionString("SQLConnection"));
            }
        }

        public virtual DbSet<MoUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<MoUser>(entity =>
            {
                entity.ToTable("Tb_Usuario");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasColumnName("Nombre")
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Passwd)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
