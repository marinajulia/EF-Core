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
            // FuncoesDeDatas();
            // FuncaoLike();
            // FuncaoDataLength();
            // FuncaoProperty();
            // FuncaoCollate();
            // TesteInterceptacao();
            // ComportamentoPadrao();
            // GerenciandoTransacaoManualmente();
            ReverterTransacao();
        }

        static void ReverterTransacao(){

            CadastrarLivros();
            using(var db = new ApplicationContext()){
                var transaao = db.Database.BeginTransaction();

                try{
                    var livro = db.Livros.FirstOrDefault(p=>p.Id == 1);
                    livro.Autor = "Rafael Almeida";
                    db.SaveChanges();

                    db.Livros.Add(
                        new Livro{
                            Titulo = "Dominando o EFCore",
                            Autor = "Rafael Almeida".PadLeft(16, '*')
                        }
                    );
                    db.SaveChanges();
                    transaao.Commit();
                }catch(Exception e){
                    transaao.Rollback();
                }
                
            }
        }
        static void GerenciandoTransacaoManualmente(){

            CadastrarLivros();
            using(var db = new ApplicationContext()){
                var transaao = db.Database.BeginTransaction();

                var livro = db.Livros.FirstOrDefault(p=>p.Id == 1);
                livro.Autor = "Rafael Almeida";
                db.SaveChanges();

                Console.ReadKey();

                db.Livros.Add(
                    new Livro{
                        Titulo = "Dominando o EFCore",
                        Autor = "Rafael Almeida"
                    }
                );
                db.SaveChanges();
                transaao.Commit();
            }
        }
        static void ComportamentoPadrao(){

            CadastrarLivros();
            using(var db = new ApplicationContext()){
                var livro = db.Livros.FirstOrDefault(p=>p.Id == 1);
                livro.Autor = "Rafael Almeida";

                db.Livros.Add(
                    new Livro{
                        Titulo = "Dominando o EFCore",
                        Autor = "Rafael Almeida"
                    }
                );
                db.SaveChanges();
            }
        }
        static void CadastrarLivros(){
            using(var db = new ApplicationContext()){
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Livros.Add(
                    new Livro{
                        Titulo = "Introdução ao EFCore",
                        Autor = "Rafael"
                    }
                );

                db.SaveChanges();
            }
        }
        static void TesteInterceptacao(){
            using(var db = new ApplicationContext()){
                var consulta = db.Funcoes.FirstOrDefault();
                System.Console.WriteLine($"Consulta: {consulta?.Descricao1}");
            }
        }
        static void FuncaoCollate(){
            
            ApagarCriarBancoDeDados();

            using(var db = new ApplicationContext()){

                var consulta1 = db.Funcoes
                .FirstOrDefault(p=> EF.Functions.Collate(p.Descricao1, "SQL_Latin1_General_CP1_CS_AS") == "tela");

                var consulta2 = db.Funcoes
                .FirstOrDefault(p=> EF.Functions.Collate(p.Descricao1, "SQL_Latin1_General_CP1_CI_AS") == "tela");

                System.Console.WriteLine($"Consulta1: {consulta1?.Descricao1}");
                System.Console.WriteLine($"Consulta2: {consulta2?.Descricao1}");
            }
        }
        static void FuncaoProperty(){

            ApagarCriarBancoDeDados();

            using(var db = new ApplicationContext()){

                var resultado = db.Funcoes
                // .AsNoTracking()
                .FirstOrDefault(p=> EF.Property<string>(p, "PropriedadeSombra") == "Teste");

                var propriedadeSombra = db.Entry(resultado)
                    .Property<string>("PropriedadeSombra")
                    .CurrentValue;

                System.Console.WriteLine("Resultado:");
                System.Console.WriteLine(propriedadeSombra);
            }

        }
        static void FuncaoDataLength(){
            using(var db = new ApplicationContext()){

                var resultado = db.Funcoes
                    .AsNoTracking()
                    .Select(p=> new{
                        TotalBytesCampoData = EF.Functions.DataLength(p.Data1),
                        TotalBytes1 = EF.Functions.DataLength(p.Descricao1),
                        TotalBytes2 = EF.Functions.DataLength(p.Descricao2),
                        Total1 = p.Descricao1.Length,
                        Total2 = p.Descricao2.Length
                    }).FirstOrDefault();

                System.Console.WriteLine("Resultado:");
                System.Console.WriteLine(resultado);
            }
        }
        static void FuncaoLike(){
            using(var db = new ApplicationContext()){
                var script = db.Database.GenerateCreateScript();
                System.Console.WriteLine(script);

                var dados = db.Funcoes
                    .AsNoTracking()
                    // .Where(p=> EF.Functions.Like(p.Descricao1, "%Bo%"))
                    .Where(p=> EF.Functions.Like(p.Descricao1, "B[ao]%"))
                    .Select(p=> p.Descricao1)
                    .ToArray();
                
                System.Console.WriteLine("Resultado: ");
                foreach(var descricao in dados){
                    System.Console.WriteLine(descricao);
                }
            }

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
                Descricao1 = "Tela",
                Descricao2 = "Tela"
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

