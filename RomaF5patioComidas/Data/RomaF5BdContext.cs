﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RomaF5patioComidas.Models;

namespace RomaF5patioComidas.Data
{
    public partial class RomaF5BdContext : DbContext
    {
        public RomaF5BdContext()
        {
        }

        public RomaF5BdContext(DbContextOptions<RomaF5BdContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bebida> Bebida { get; set; }
        public virtual DbSet<CategoriaUser> CategoriaUser { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Mesa> Mesa { get; set; }
        public virtual DbSet<Pedido> Pedido { get; set; }
        public virtual DbSet<TipoBebida> TipoBebida { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bebida>(entity =>
            {
                entity.HasKey(e => e.IdBebida);

                entity.Property(e => e.IdBebida).HasColumnName("idBebida");

                entity.Property(e => e.Eliminar).HasColumnName("eliminar");

                entity.Property(e => e.IdTipobebida).HasColumnName("idTipobebida");

                entity.Property(e => e.Marca)
                    .HasMaxLength(50)
                    .HasColumnName("marca");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .HasColumnName("nombre");

                entity.Property(e => e.Precio).HasColumnName("precio");

                entity.Property(e => e.Stock).HasColumnName("stock");

                entity.HasOne(d => d.IdTipobebidaNavigation)
                    .WithMany(p => p.Bebida)
                    .HasForeignKey(d => d.IdTipobebida)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bebida_TipoBebida");
            });

            modelBuilder.Entity<CategoriaUser>(entity =>
            {
                entity.HasKey(e => e.IdCategoria);

                entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");

                entity.Property(e => e.Categoria)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("categoria");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.IdMenu);

                entity.Property(e => e.IdMenu).HasColumnName("idMenu");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Eliminar).HasColumnName("eliminar");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .HasColumnName("nombre");

                entity.Property(e => e.Precio).HasColumnName("precio");
            });

            modelBuilder.Entity<Mesa>(entity =>
            {
                entity.HasKey(e => e.IdMesa);

                entity.Property(e => e.IdMesa).HasColumnName("idMesa");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Eliminar).HasColumnName("eliminar");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.NombreReserva)
                    .HasMaxLength(50)
                    .HasColumnName("nombreReserva");

                entity.Property(e => e.Reserva).HasColumnName("reserva");
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => e.IdPedido);

                entity.Property(e => e.IdPedido).HasColumnName("idPedido");

                entity.Property(e => e.CantidadBebida).HasColumnName("cantidadBebida");

                entity.Property(e => e.CantidadMenu).HasColumnName("cantidadMenu");

                entity.Property(e => e.Eliminar).HasColumnName("eliminar");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdBebida).HasColumnName("idBebida");

                entity.Property(e => e.IdMesa).HasColumnName("idMesa");

                entity.Property(e => e.Idmenu).HasColumnName("idmenu");

                entity.Property(e => e.Total).HasColumnName("total");

                entity.HasOne(d => d.IdBebidaNavigation)
                    .WithMany(p => p.Pedido)
                    .HasForeignKey(d => d.IdBebida)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pedido_Bebida");

                entity.HasOne(d => d.IdMesaNavigation)
                    .WithMany(p => p.Pedido)
                    .HasForeignKey(d => d.IdMesa)
                    .HasConstraintName("FK_Pedido_Mesa");

                entity.HasOne(d => d.IdmenuNavigation)
                    .WithMany(p => p.Pedido)
                    .HasForeignKey(d => d.Idmenu)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pedido_Menu");
            });

            modelBuilder.Entity<TipoBebida>(entity =>
            {
                entity.HasKey(e => e.IdTipobebida);

                entity.Property(e => e.IdTipobebida).HasColumnName("idTipobebida");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Eliminar).HasColumnName("eliminar");

                entity.Property(e => e.Litros).HasColumnName("litros");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("clave");

                entity.Property(e => e.Eliminar).HasColumnName("eliminar");

                entity.Property(e => e.IdCategoria).HasColumnName("idCategoria");

                entity.Property(e => e.Usuario1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("usuario");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.IdCategoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_CategoriaUser");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}