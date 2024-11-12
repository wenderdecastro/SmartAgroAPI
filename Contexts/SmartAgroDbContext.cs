using System;
using System.Collections.Generic;
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

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<LogsSensor> LogsSensors { get; set; }

    public virtual DbSet<NotificacaoStatus> NotificacaoStatuses { get; set; }

    public virtual DbSet<Notificaco> Notificacoes { get; set; }

    public virtual DbSet<Sensor> Sensors { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=smartagroserver.database.windows.net;Database=SmartAgroDB;User Id=SmartAgro;pwd=Grupo6DB;Trusted_Connection=False;Encrypt=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorium>(entity =>
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

        modelBuilder.Entity<NotificacaoStatus>(entity =>
        {
            entity.ToTable("NotificacaoStatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Notificaco>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DataCriacao).HasColumnType("datetime");
            entity.Property(e => e.Mensagem).HasColumnType("text");
            entity.Property(e => e.Propriedade)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.Sensor).WithMany(p => p.Notificacos)
                .HasForeignKey(d => d.SensorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificacoes_Sensor");

            entity.HasOne(d => d.Status).WithMany(p => p.Notificacos)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificacoes_NotificacaoStatus");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Notificacos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificacoes_Usuario");
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

            entity.HasOne(d => d.Categoria).WithMany(p => p.Sensors)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Plantacao_Categoria");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Sensors)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sensor_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Usuario_1");

            entity.ToTable("Usuario");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CodigoVerificacao)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.ExpiracaoCodigo).HasColumnType("datetime");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Senha).HasMaxLength(128);
            entity.Property(e => e.Telefone).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
