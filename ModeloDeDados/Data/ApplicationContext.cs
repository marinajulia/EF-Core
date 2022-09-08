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
        //o append serve para não criar um arquivo novo toda vez, e sim reaproveitar o mesmo
        private readonly StreamWriter _writer = new StreamWriter("meu_log_do_ef_core.txt", append: true);
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Conversor> Conversores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Ator> Atores { get; set; }
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Documento> Documentos { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Instrutor> Instrutores { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Atributo> Atributos { get; set; }
        public DbSet<Dictionary<string, object>> Configuracoes => Set<Dictionary<string, object>>("Configuracoes");

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
            // modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");//caseinsensitive, ignora acentuação

            // modelBuilder.Entity<Departamento>().Property(p=>p.Descricao)
            // .UseCollation("SQL_Latin1_General_CP1_CS_AS"); //case sensitive e valida acentuação

            // modelBuilder.HasSequence<int>("MinhaSequencia", "sequencias")
            // .StartsAt(1)
            // .IncrementsBy(2)
            // .HasMin(1)
            // .HasMax(10)
            // .IsCyclic();

            // modelBuilder.Entity<Departamento>().Property(p=>p.Id)
            // .HasDefaultValueSql("NEXT VALUE FOR sequencias.MinhaSequencia"); 

            // modelBuilder.Entity<Departamento>()
            // .HasIndex(p=> new {p.Descricao, p.Ativo})
            // .HasDatabaseName("idx_meu_indice_composto")
            // .HasFilter("Descricao IS NOT NULL")
            // .HasFillFactor(80)
            // .IsUnique();

            // modelBuilder.Entity<Estado>().HasData(new[]{
            //     new Estado{ Id = 1, Nome = "São Paulo"},
            //     new Estado{ Id = 2, Nome = "Sergipe"}
            // });

            // modelBuilder.HasDefaultSchema("cadastros");
            // modelBuilder.Entity<Estado>().ToTable("Estados", "Segundo esquema");

            // var conversao = new ValueConverter<Versao, string>(p=> p.ToString(), p=> (Versao)Enum.Parse(typeof(Versao), p));
            // var conversao1 = new EnumToStringConverter<Versao>();

            // modelBuilder.Entity<Conversor>()
            // .Property (p=> p.Versao)
            // .HasConversion(conversao1);
            // // .HasConversion<string>();

            // modelBuilder.Entity<Conversor>()
            // .Property (p=> p.Status)
            // .HasConversion(new ConversorCustomizado());

            // modelBuilder.Entity<Departamento>().Property<DateTime>("UltimaAtualizacao");

            // modelBuilder.Entity<Cliente>(p=>
            // {
            //     p.OwnsMany(x=> x.Endereco);
            // });

            // modelBuilder.ApplyConfiguration(new ClientConfiguration());
            // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Configuracoes", b =>{

                b.Property<int>("Id");

                b.Property<string>("Chave")
                    .HasColumnType("VARCHAR(40)")
                    .IsRequired();
                    
                b.Property<string>("Valor")
                    .HasColumnType("VARCHAR(255)")
                    .IsRequired();
            });
        }
       
    }
}