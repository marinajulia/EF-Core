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
            // EnsureCreatedAndDeleted();
            // GapEnsureCreated();
            // HealthCheckBancoDeDados();
            // SqlInjection();
            // MigracoesPendentes();
            // AplicarMigracaoEmTempoDeExecucao();
            // TodasMigracoes();
            // MigracoesJaAplicadas();
            // ScriptGeralDoBancoDeDados();
            // CarregamentoAdiantado();
            // CarregamentoExplicito();
            // CarregamentoLento();
            // FiltroGlobal();
            // IgnoreFiltroGlobal();
            // ConsultaProjetada();
            // ConsultaParametrizada();
            // ConsultaInterpolada();
            // ConsultaComTag();
            // EntendendoConsulta1NN1();
            // DivisaodeConsulta();
            // CriarStoredProcedure();
            // InserirDadosViaProcedure();
            // CriarStoredProcedureDeConsulta();
            ConsultaViaProcedure();
        }

        static void ConsultaViaProcedure(){
            using var db = new ApplicationContext();
            var dep = new SqlParameter("@dep", "Departamento");

            var departamentos = db.Departamentos.FromSqlRaw("EXECUTE GetDepartamentos @dep", dep)
            .ToList();

            foreach(var departamento in departamentos){
                Console.WriteLine(departamento.Descricao);
            }
        }
        static void CriarStoredProcedureDeConsulta(){
            var criarDepartamento = @"
            CREATE OR ALTER PROCEDURE GetDepartamentos
                @Descricao VARCHAR(50)
            AS
            BEGIN
                SELECT * FROM Departamentos Where Descricao like @Descricao + '%'
            END
            ";

            using var db = new ApplicationContext();
            db.Database.ExecuteSqlRaw(criarDepartamento);
        }
        static void InserirDadosViaProcedure(){
            using var db = new ApplicationContext();
            db.Database.ExecuteSqlRaw("execute CriarDepartamento @p0, @p1", "Departamento Via Procedure", true);
        }
        static void CriarStoredProcedure(){
            var criarDepartamento = @"
            CREATE OR ALTER PROCEDURE CriarDepartamento
                @Descricao VARCHAR(50),
                @Ativo bit
            AS
            BEGIN
                INSERT INTO
                    Departamentos(Descricao, Ativo, Excluido)
                    VALUES(@Descricao, @Ativo, 0)
            END
            ";

            using var db = new ApplicationContext();
            db.Database.ExecuteSqlRaw(criarDepartamento);
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
            // using var db1 = new ApplicationContext(); 
            // using var db2 = new ApplicationContextCidade(); 

            // db1.Database.EnsureCreated();
            // db2.Database.EnsureCreated();

            // var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();
            // databaseCreator.CreateTables();
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
        static void MigracoesPendentes(){
            using var db = new ApplicationContext();
            var migracoesPendentes = db.Database.GetPendingMigrations();
            Console.WriteLine($"Total: {migracoesPendentes.Count()}");

            foreach(var migracao in migracoesPendentes){
                Console.WriteLine($"Migração:{migracao}");
            }
        }
        static void AplicarMigracaoEmTempoDeExecucao(){
            using var db = new ApplicationContext();
            db.Database.Migrate();
        }
        static void TodasMigracoes(){
            using var db = new ApplicationContext();
            var migracoes = db.Database.GetMigrations();

            Console.WriteLine($"Total: {migracoes.Count()}");

            foreach(var migracao in migracoes){
                Console.WriteLine($"Migração: {migracao}");
            }
        }
        static void MigracoesJaAplicadas(){
            using var db = new ApplicationContext();
            var migracoes = db.Database.GetAppliedMigrations();

            Console.WriteLine($"Total: {migracoes.Count()}");

            foreach(var migracao in migracoes){
                Console.WriteLine($"Migração: {migracao}");
            }
        }
        static void ScriptGeralDoBancoDeDados(){
            using var db = new ApplicationContext();
            var script = db.Database.GenerateCreateScript();

            Console.WriteLine($"script: {script}");
        }
        static void SetupTiposCarregamentos(ApplicationContext db){
            if(!db.Departamentos.Any()){
                db.Departamentos.AddRange(
                    new Departamento{
                        Descricao = "Departamento 01",
                        Funcionarios = new List<Funcionario>{
                            new Funcionario{
                                Nome = "Rafael Almeida",
                                CPF = "865435765544",
                                RG = "674764756"
                            }
                        }
                    },
                    new Departamento{
                        Descricao = "Departamento 02",
                        Funcionarios = new List<Funcionario>{
                            new Funcionario{
                                Nome = "Rafael Almeida2",
                                CPF = "865435765544",
                                RG = "674764756"
                            },
                            new Funcionario{
                                Nome = "Rafael Almeida3",
                                CPF = "865435765544",
                                RG = "674764756"
                            }
                        }
                    }
                );
                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }
        static void CarregamentoAdiantado(){
            using var db = new ApplicationContext();
            SetupTiposCarregamentos(db);

            var departamentos = db.Departamentos.Include(p=> p.Funcionarios);

            foreach(var departamento in departamentos){
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"Departamneto: {departamento.Descricao}");

                if(departamento.Funcionarios?.Any() ?? false){
                    foreach(var funcionario in departamento.Funcionarios){
                        Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
                    }
                }else{
                    Console.WriteLine($"\tNenhum funcionario encontrado");
                }
            }
        }
        static void CarregamentoExplicito(){
            using var db = new ApplicationContext();
            SetupTiposCarregamentos(db);

            var departamentos = db.Departamentos.ToList();

            foreach(var departamento in departamentos){
                if(departamento.Id == 2){
                    // db.Entry(departamento).Collection(p=> p.Funcionarios).Load();
                    db.Entry(departamento).Collection(p=> p.Funcionarios).Query().Where(p=> p.Id > 2).ToList();
                }
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"Departamneto: {departamento.Descricao}");

                if(departamento.Funcionarios?.Any() ?? false){
                    foreach(var funcionario in departamento.Funcionarios){
                        Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
                    }
                }else{
                    Console.WriteLine($"\tNenhum funcionario encontrado");
                }
            }
        }
        static void CarregamentoLento(){
            using var db = new ApplicationContext();
            SetupTiposCarregamentos(db);
            // db.ChangeTracker.LazyLoadingEnabled = false;
            var departamentos = db.Departamentos.ToList();

            foreach(var departamento in departamentos){
                
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"Departamneto: {departamento.Descricao}");

                if(departamento.Funcionarios?.Any() ?? false){
                    foreach(var funcionario in departamento.Funcionarios){
                        Console.WriteLine($"\tFuncionario: {funcionario.Nome}");
                    }
                }else{
                    Console.WriteLine($"\tNenhum funcionario encontrado");
                }
            }
        }
        static void FiltroGlobal(){
            using var db = new ApplicationContext();
            Setup(db);

            var departamentos = db.Departamentos.Where(p=> p.Id > 0).ToList();

            foreach(var departamento in departamentos){
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}");
            }
        }
        static void IgnoreFiltroGlobal(){
            using var db = new ApplicationContext();
            Setup(db);

            var departamentos = db.Departamentos.IgnoreQueryFilters().Where(p=> p.Id > 0).ToList();

            foreach(var departamento in departamentos){
                Console.WriteLine($"Descrição: {departamento.Descricao} \t Excluido: {departamento.Excluido}");
            }
        }
        static void ConsultaProjetada(){
            using var db = new ApplicationContext();
            Setup(db);

            var departamentos = db.Departamentos
            .Where(p=> p.Id > 0)
            .Select(p=> new {p.Descricao, Funcionarios = p.Funcionarios.Select(f =>f.Nome)})
            .ToList();

            foreach(var departamento in departamentos){
                Console.WriteLine($"Descrição: {departamento.Descricao}");
                foreach(var funcionario in departamento.Funcionarios){
                Console.WriteLine($"Nome: {funcionario}");
            }
            }
        }
        static void ConsultaParametrizada(){
            using var db = new ApplicationContext();
            Setup(db);

            var id = new SqlParameter{
                Value = 1,
                SqlDbType = SqlDbType.Int
            };
            
            var departamentos = db.Departamentos
            .FromSqlRaw("SELECT * FROM  Departamentos WHERE Id>{0}", 1)
            .Where(p=>!p.Excluido)
            .ToList();

            foreach(var departamento in departamentos){
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }
        static void ConsultaInterpolada(){
            using var db = new ApplicationContext();
            Setup(db);

            var id = 1;
            
            var departamentos = db.Departamentos
            .FromSqlInterpolated($"SELECT * FROM  Departamentos WHERE Id>{id}")
            .ToList();

            foreach(var departamento in departamentos){
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }

        static void ConsultaComTag(){
            using var db = new ApplicationContext();
            Setup(db);

            var departamentos = db.Departamentos
            .TagWith(@"Estou enviando um comentario para o servidor
                segundo comentario
                terceiro comentario")
            .ToList();

            foreach(var departamento in departamentos){
                Console.WriteLine($"Descrição: {departamento.Descricao}");
            }
        }
        static void EntendendoConsulta1NN1(){
            using var db = new ApplicationContext();
            Setup(db);

            // var departamentos = db.Departamentos
            // .Include(p=>p.Funcionarios)
            // .ToList();

            // foreach(var departamento in departamentos){
            //     Console.WriteLine($"Descrição: {departamento.Descricao}");

            //     foreach(var funcionario in departamento.Funcionarios){
            //     Console.WriteLine($"Nome: {funcionario.Nome}");
            // }
            // }
            var funcionarios = db.Funcionarios
            .Include(p=>p.Departamento)
            .ToList();

                foreach(var funcionario in funcionarios){
                Console.WriteLine($"Nome: {funcionario.Nome} / descrição dep:{funcionario.Departamento.Descricao}");
            }
        }
        static void DivisaodeConsulta(){
            using var db = new ApplicationContext();
            Setup(db);

            var departamentos = db.Departamentos
            .Include(p=>p.Funcionarios)
            // .AsSplitQuery()
            .AsSingleQuery() //para não repreduzir a single query global
            .ToList();

            foreach(var departamento in departamentos){
                Console.WriteLine($"Descrição: {departamento.Descricao}");
                foreach(var funcionario in departamento.Funcionarios){
                Console.WriteLine($"Nome: {funcionario.Nome}");
            }
            }
        }
        static void Setup(ApplicationContext db){

            if(db.Database.EnsureCreated())
                db.Departamentos.AddRange(
                    new Departamento{
                        Descricao = "Departamento 01",
                        Funcionarios = new List<Funcionario>{
                            new Funcionario{
                                Nome = "Rafael Almeida",
                                CPF = "865435765544",
                                RG = "674764756"
                            }
                        },
                    Excluido=true
                    },
                    new Departamento{
                        Descricao = "Departamento 02",
                        Funcionarios = new List<Funcionario>{
                            new Funcionario{
                                Nome = "Rafael Almeida2",
                                CPF = "865435765544",
                                RG = "674764756"
                            },
                            new Funcionario{
                                Nome = "Rafael Almeida3",
                                CPF = "865435765544",
                                RG = "674764756"
                            }
                        },
                    }
                );
                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
        }
    }

