using System;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Curso.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
            .UseSqlServer(@"Data Source=DESKTOP-RTPBNVC\SQLEXPRESS;Initial Catalog=Curso;Integrated Security=True;pooling=true;")
            .EnableSensitiveDataLogging()
            // .UseLazyLoadingProxies()
            .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}