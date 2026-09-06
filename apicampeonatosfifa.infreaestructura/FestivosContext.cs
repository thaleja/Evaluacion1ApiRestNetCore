using Microsoft.EntityFrameworkCore;
using apicampeonatosfifa.dominio;

namespace apicampeonatosfifa.infraestructura
{
    public class FestivosContext : DbContext
    {
        public FestivosContext(DbContextOptions<FestivosContext> options) : base(options)
        {
        }

        public DbSet<Pais> Paises { get; set; }
        public DbSet<TipoFestivo> Tipos { get; set; }
        public DbSet<Festivo> Festivos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeamos explícitamente las clases limpias a las tablas de la Base de Datos
            modelBuilder.Entity<Pais>().ToTable("Pais");
            modelBuilder.Entity<TipoFestivo>().ToTable("Tipo");
            modelBuilder.Entity<Festivo>().ToTable("Festivo");

            // Configuramos las relaciones entre las tablas de Festivos
            modelBuilder.Entity<Festivo>()
                .HasOne(f => f.Tipo)
                .WithMany()
                .HasForeignKey(f => f.IdTipo);

            modelBuilder.Entity<Festivo>()
                .HasOne(f => f.Pais)
                .WithMany()
                .HasForeignKey(f => f.IdPais);
        }
    }
}