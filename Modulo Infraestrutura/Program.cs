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
            // ConsultarDepartamentos();
            // DadosSensiveis();
            HabilitandoBatchSize();
        }
        static void HabilitandoBatchSize(){
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            for( var i = 0; i<50; i++){
                db.Departamentos.Add(
                    new Departamento{
                        Descricao = "Departamento " + i,
                        Ativo = true,
                        Excluido = false
                    }
                );
            }
            db.SaveChanges();
        }
        static void DadosSensiveis(){
            using var db = new ApplicationContext();
            var descricao = "Departamento";
            var departamentos = db.Departamentos.Where(p=> p.Descricao ==descricao).ToArray(); 
        }
        static void ConsultarDepartamentos(){
            using var db = new ApplicationContext();
            var departamentos = db.Departamentos.Where(p=> p.Id > 0).ToArray(); 
        }
    }
}

