using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Curso.Data;
using Curso.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DominandoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // Collactions();
            // PropragarDados();
            // Esquema();
            // ConversoresDeValor();
            // ConversorCustomizado();
            // PropriedadeDeSombra();
            // TrabalhandoComPropriedadeDeSombra();
            // Relacionamento1Para1();
            RelacionamentoMuitosParaMuitos();
        }
        static void RelacionamentoMuitosParaMuitos()
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var ator1 = new Ator { Nome = "Ator1" };
                var ator2 = new Ator { Nome = "Ator2" };
                var ator3 = new Ator { Nome = "Ator3" };

                var filme1 = new Filme { Descricao = "Descricao1" };
                var filme2 = new Filme { Descricao = "Descricao2" };
                var filme3 = new Filme { Descricao = "Descricao3" };

                ator1.Filmes.Add(filme1);
                ator1.Filmes.Add(filme2);

                ator2.Filmes.Add(filme1);

                filme3.Atores.Add(ator1);
                filme3.Atores.Add(ator2);
                filme3.Atores.Add(ator3);

                db.AddRange(ator1, ator2, filme3);

                db.SaveChanges();

                foreach (var ator in db.Atores.Include(p => p.Filmes))
                {
                    Console.WriteLine($"Ator: {ator.Nome}");

                    foreach (var filme in ator.Filmes)
                    {
                        Console.WriteLine($"Filme: {filme.Descricao}");
                    }
                }
            }

        }
        static void Relacionamento1ParaMuitos()
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var estado = new Estado
                {
                    Nome = "Sergipe",
                    Governador = new Governador { Nome = "Rafael Almeida" }
                };
                estado.Cidades.Add(new Cidade { Nome = "Teste" });

                db.Estados.Add(estado);
                db.SaveChanges();
            }

            using (var db = new ApplicationContext())
            {
                var estados = db.Estados.AsNoTracking().ToList();
                estados[0].Cidades.Add(new Cidade { Nome = "Aracaju" });

                db.SaveChanges();

                foreach (var est in db.Estados.Include(p => p.Cidades).AsNoTracking())
                {
                    System.Console.WriteLine($"Estado: {est.Nome}, Governador: {est.Governador.Nome}");
                    foreach (var cidade in est.Cidades)
                    {
                        System.Console.WriteLine($"Cidade: {cidade.Nome}");
                    }
                }
            }
        }

        static void Relacionamento1Para1()
        {
            using var db = new ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var estado = new Estado
            {
                Nome = "Sergipe",
                Governador = new Governador { Nome = "Rafael Almeida" }
            };
            db.Estados.Add(estado);
            db.SaveChanges();

            var estados = db.Estados.AsNoTracking().ToList();

            estados.ForEach(est =>
            {
                System.Console.WriteLine($"Estado: {est.Nome}, Governador: {est.Governador.Nome}");
            });
        }
        static void TrabalhandoComPropriedadeDeSombra()
        {
            using var db = new ApplicationContext();
            // db.Database.EnsureDeleted();
            // db.Database.EnsureCreated();

            // var departamento = new Departamento{
            //     Descricao = "Departamento Propriedade De Sombra"
            // };

            // db.Departamentos.Add(departamento);
            // db.Entry(departamento).Property("UltimaAtualizacao").CurrentValue = DateTime.Now;
            // db.SaveChanges();

            var departamentos = db.Departamentos.Where(p => EF.Property<DateTime>(p, "UltimaAtualizacao") < DateTime.Now).ToArray();
        }
        static void PropriedadeDeSombra()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
        static void ConversorCustomizado()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Conversores.Add(
                new Conversor
                {
                    Status = Status.Devolvido,
                }
            );

            db.SaveChanges();

            var conversorEmAnalise = db.Conversores.AsNoTracking().FirstOrDefault(p => p.Status == Status.Analise);
            var conversorDevolvido = db.Conversores.AsNoTracking().FirstOrDefault(p => p.Status == Status.Devolvido);
        }
        static void Collactions()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
        static void PropragarDados()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var script = db.Database.GenerateCreateScript();
            System.Console.WriteLine(script);
        }

        static void Esquema()
        {
            using var db = new ApplicationContext();

            var script = db.Database.GenerateCreateScript();
            System.Console.WriteLine(script);
        }

        static void ConversoresDeValor() => Esquema();
    }
}

