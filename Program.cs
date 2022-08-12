using System;
using Curso.Data;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DominandoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // EnsureCreatedAndDeleted();
            // GapEnsureCreated();
            // HealthCheckBancoDeDados();
            SqlInjection();
        }

        static void HealthCheckBancoDeDados(){
            using var db = new ApplicationContext();
            var canConnect = db.Database.CanConnect(); //Serve para validar se consegue conectar a base de dados
            if(canConnect){
                var connection = db.Database.GetDbConnection();
                connection.Open();

                Console.WriteLine("Posso me conectar");
            }else{
                Console.WriteLine("Não posso me conectar");
            }
        }
        static void EnsureCreatedAndDeleted(){
            using var db = new ApplicationContext();
            // db.Database.EnsureCreated();
            db.Database.EnsureDeleted();
        }
        static void GapEnsureCreated(){
            using var db1 = new ApplicationContext(); 
            using var db2 = new ApplicationContextCidade(); 

            db1.Database.EnsureCreated();
            db2.Database.EnsureCreated();

            var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();
            databaseCreator.CreateTables();
        }
        static void ExecuteSQL(){
            using var db = new ApplicationContext();

            //primeira opção:
            using(var cmd = db.Database.GetDbConnection().CreateCommand()){
                cmd.CommandText = "SELECT 1";
                cmd.ExecuteNonQuery();
            }

            //Segunda opção:
            var descricao = "TESTE";
            db.Database.ExecuteSqlRaw("update departamentos set descricao={0}, where id=1", descricao);

            //Terceira opção:
            db.Database.ExecuteSqlInterpolated($"update departamentos set descricao={descricao}, where id=1");
        }
        static void SqlInjection(){
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Departamentos.AddRange(
                new Departamento{
                    Descricao = "Departamento 01"
                },
                new Departamento{
                    Descricao = "Departamento 02"
                }
            );
            db.SaveChanges();
            var descricao = "Teste ' or 1='1";
            db.Database.ExecuteSqlRaw($"update departamentos set descricao='AtaqueSQLInjection' where descricao='{descricao}'");

            foreach(var Departamento in db.Departamentos.AsNoTracking()){
                System.Console.WriteLine($"Id: {Departamento.Id}, Descrição: {Departamento.Descricao}");
            }
        }
    }
}
