using API_FACTURACION_NET_CORE.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace API_FACTURACION_NET_CORE.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Invoice> Invoices => Set<Invoice>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoices");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.ClaveAcceso)
                    .HasMaxLength(49)
                    .IsRequired();

                entity.Property(x => x.Ruc)
                    .HasMaxLength(13)
                    .IsRequired();

                entity.Property(x => x.CodDoc)
                    .HasMaxLength(2)
                    .IsRequired();

                entity.Property(x => x.Estab)
                    .HasMaxLength(3)
                    .IsRequired();

                entity.Property(x => x.PtoEmi)
                    .HasMaxLength(3)
                    .IsRequired();

                entity.Property(x => x.Secuencial)
                    .HasMaxLength(9)
                    .IsRequired();

                entity.Property(x => x.Estado)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(x => x.PayloadJson)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired();

                entity.Property(x => x.ErrorMessage)
                    .HasColumnType("nvarchar(max)");

                entity.HasIndex(x => x.ClaveAcceso)
                    .IsUnique();

                entity.HasIndex(x => x.Estado);

                entity.Property(x => x.XmlPath)
                    .HasMaxLength(500);

                entity.Property(x => x.XmlContent)
                    .HasColumnType("nvarchar(max)");

                entity.Property(x => x.SriAuthorizationNumber)
                    .HasMaxLength(100);
            });
        }
    }
}
