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
            ConsultarDepartamentos();
        }

        static void ConsultarDepartamentos(){
            using var db = new ApplicationContext();
            var departamentos = db.Departamentos.Where(p=> p.Id > 0).ToArray(); 
        }
    }
}

