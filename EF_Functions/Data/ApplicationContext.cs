using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Curso.Configurations;
using Curso.Conversores;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;

namespace Curso.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Funcao> Funcoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = @"Data Source=DESKTOP-RTPBNVC\SQLEXPRESS;Initial Catalog=Curso;Integrated Security=True;pooling=true;";
            optionsBuilder
                .UseSqlServer(strConnection)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                ;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder
            .Entity<Funcao>(conf =>{
                conf.Property<string>("PropriedadeSombra")
                .HasColumnType("VARCHAR(100)")
                .HasDefaultValueSql("'Teste'");
            });
        }
       
    }
}