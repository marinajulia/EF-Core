using System;
using System.Data;
using System.Linq;
using Curso.Data;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;

namespace DominandoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            FuncoesDeDatas();
        }

        static void ApagarCriarBancoDeDados()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Funcoes.AddRange(new Funcao{
                Data1 = DateTime.Now.AddDays(2),
                Data2 = "2021-04-08",
                Descricao1 = "Bala 1",
                Descricao2 = "Bala 1"
            },
            new Funcao{
                Data1 = DateTime.Now.AddDays(1),
                Data2 = "XX21-04-08",
                Descricao1 = "Bola 2",
                Descricao2 = "Bola 2"
            },
            new Funcao{
                Data1 = DateTime.Now.AddDays(1),
                Data2 = "XX21-04-08",
                Descricao1 = "Tela 1",
                Descricao2 = "Tela 2"
            });
            db.SaveChanges();
               
        }
        static void FuncoesDeDatas(){
            ApagarCriarBancoDeDados ();
            using(var db = new ApplicationContext()){
                var script = db.Database.GenerateCreateScript();
                Console.WriteLine(script);

                var dados = db.Funcoes.AsNoTracking().Select(p=>
                    new{
                        Dias = EF.Functions.DateDiffDay(DateTime.Now, p.Data1),
                        Data = EF.Functions.DateFromParts(2021, 1, 2),
                        DataValida = EF.Functions.IsDate(p.Data2),
                    });

                foreach(var f in dados){
                    Console.WriteLine(f);
                }
            }
        }
    }
}

