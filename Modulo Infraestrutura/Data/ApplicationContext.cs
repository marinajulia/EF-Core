using System;
using System.IO;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Curso.Data
{
    public class ApplicationContext : DbContext
    {
        //o append serve para não criar um arquivo novo toda vez, e sim reaproveitar o mesmo
        private readonly StreamWriter _writer = new StreamWriter("meu_log_do_ef_core.txt", append: true);
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = @"Data Source=DESKTOP-RTPBNVC\SQLEXPRESS;Initial Catalog=Curso;Integrated Security=True;pooling=true;";
            optionsBuilder
                .UseSqlServer(strConnection)
                // .LogTo(Console.WriteLine, LogLevel.Information);
                // .LogTo(Console.WriteLine, 
                //     new[] {CoreEventId.ContextInitialized, RelationalEventId.CommandExecuted},
                //     LogLevel.Information,
                //     DbContextLoggerOptions.LocalTime | DbContextLoggerOptions.SingleLine
                // );
                .LogTo(_writer.WriteLine, LogLevel.Information);
        }

        public override void Dispose(){
            base.Dispose();
            _writer.Dispose();
        }
       
    }
}