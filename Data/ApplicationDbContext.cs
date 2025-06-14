using Microsoft.EntityFrameworkCore;
using ProjetoESG.Models;

namespace ProjetoESG.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<EnergyConsumption> EnergyConsumptions { get; set; }
        public DbSet<Alert> Alerts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações para EnergyConsumption
            modelBuilder.Entity<EnergyConsumption>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ConsumptionKwh).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(300);
                entity.Property(e => e.CreatedAt).IsRequired();
                
                entity.HasIndex(e => e.Timestamp);
                entity.HasIndex(e => e.CompanyName);
                entity.HasIndex(e => e.Location);
            });

            // Configurações para Alert
            modelBuilder.Entity<Alert>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.ConsumptionKwh).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Severity).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(300);
                entity.Property(e => e.IsResolved).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.ResolvedAt);
                
                entity.HasIndex(e => e.CreatedAt);
                entity.HasIndex(e => e.CompanyName);
                entity.HasIndex(e => e.Severity);
                entity.HasIndex(e => e.IsResolved);

                // Relacionamento com EnergyConsumption
                entity.HasOne(e => e.EnergyConsumption)
                      .WithMany()
                      .HasForeignKey(e => e.EnergyConsumptionId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
} 