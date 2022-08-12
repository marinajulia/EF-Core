using System;
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
            HealthCheckBancoDeDados();
        }

        static void HealthCheckBancoDeDados(){
            using var db = new Curso.Data.ApplicationContext();
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
            using var db = new Curso.Data.ApplicationContext();
            // db.Database.EnsureCreated();
            db.Database.EnsureDeleted();
        }
        static void GapEnsureCreated(){
            using var db1 = new Curso.Data.ApplicationContext(); 
            using var db2 = new Curso.Data.ApplicationContextCidade(); 

            db1.Database.EnsureCreated();
            db2.Database.EnsureCreated();

            var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();
            databaseCreator.CreateTables();
        }
        static void ExecuteSQL(){
            using var db = new Curso.Data.ApplicationContext();

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
    }
}
