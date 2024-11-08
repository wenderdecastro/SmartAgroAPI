using Microsoft.EntityFrameworkCore;
using SmartAgroAPI.Models;

namespace SmartAgroAPI.Contexts;

public partial class SmartAgroDbContext : DbContext
{
    public SmartAgroDbContext()
    {
    }

    public SmartAgroDbContext(DbContextOptions<SmartAgroDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<LogsSensor> LogsSensors { get; set; }

    public virtual DbSet<Sensor> Sensors { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.Property(e => e.Descricao)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LogsSensor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_DadosSensor");

            entity.ToTable("LogsSensor");

            entity.Property(e => e.DataAtualizacao).HasColumnType("datetime");
            entity.Property(e => e.Luminosidade).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.PhSolo).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.QualidadeAr).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TemperaturaAr).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.TemperaturaSolo).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.UmidadeAr).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.UmidadeSolo).HasColumnType("decimal(4, 2)");

            entity.HasOne(d => d.Sensor).WithMany(p => p.LogsSensors)
                .HasForeignKey(d => d.SensorId)
                .HasConstraintName("FK_LogsSensor_Sensor");
        });

        modelBuilder.Entity<Sensor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Plantacao");

            entity.ToTable("Sensor");

            entity.Property(e => e.Latitude).HasColumnType("decimal(8, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.LuminosidadeIdeal).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhSoloIdeal).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.TemperaturaArIdeal).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.TemperaturaSoloIdeal).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.UmidadeArIdeal).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.UmidadeSoloIdeal).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Sensors)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("FK_Plantacao_Categoria");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Sensors)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_Sensor_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Usuario_1");

            entity.ToTable("Usuario");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CodigoVerificacao)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Senha).HasMaxLength(128);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
