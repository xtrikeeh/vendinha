using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using Vendinha.Core.Models;

namespace Vendinha.Core.Data
{
    public class VendinhaDbContext : DbContext
    {
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Divida> Dividas => Set<Divida>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection"));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var modelCliente = modelBuilder.Entity<Cliente>();
            var modelDivida = modelBuilder.Entity<Divida>();

            modelCliente.ToTable("clientes");
            modelCliente.Property(e => e.Id).HasColumnName("id");
            modelCliente.Property(e => e.Nome).HasColumnName("nome");
            modelCliente.Property(e => e.Cpf).HasColumnName("cpf");
            modelCliente.Property(e => e.DataNascimento).HasColumnName("data_nascimento");
            modelCliente.Property(e => e.Status).HasColumnName("status");
            modelCliente.Property(e => e.Email).HasColumnName("email");

            modelCliente.HasKey(e => e.Id);

            modelDivida.ToTable("dividas");
            modelDivida.Property(e => e.Id).HasColumnName("id");
            modelDivida.Property(e => e.Valor).HasColumnName("valor");
            modelDivida.Property(e => e.Situacao).HasColumnName("situacao");
            modelDivida.Property(e => e.DataCriacao).HasColumnName("data_criacao");
            modelDivida.Property(e => e.DataPagamento).HasColumnName("data_pagamento");
            modelDivida.Property(e => e.ClienteId).HasColumnName("cliente_id");

            modelDivida.HasOne(e => e.Cliente).WithMany().HasForeignKey(e => e.ClienteId);
            modelDivida.HasKey(e => e.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
