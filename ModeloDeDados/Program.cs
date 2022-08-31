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
            ConversorCustomizado();
        }

        static void ConversorCustomizado(){
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Conversores.Add(
                new Conversor{
                    Status = Status.Devolvido,
                }
            );

            db.SaveChanges();

            var conversorEmAnalise = db.Conversores.AsNoTracking().FirstOrDefault(p=> p.Status == Status.Analise);
            var conversorDevolvido = db.Conversores.AsNoTracking().FirstOrDefault(p=> p.Status == Status.Devolvido);
        }
        static void Collactions(){
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
        static void PropragarDados(){
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var script = db.Database.GenerateCreateScript();
            System.Console.WriteLine(script);
        }

        static void Esquema(){
            using var db = new ApplicationContext();

            var script = db.Database.GenerateCreateScript();
            System.Console.WriteLine(script);
        }

        static void ConversoresDeValor() => Esquema();
    }
}

