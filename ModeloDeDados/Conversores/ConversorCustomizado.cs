using System;
using System.Linq;
using System.Linq.Expressions;
using Curso.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Curso.Conversores
{
    public class ConversorCustomizado : ValueConverter<Status, string>
    {
        public ConversorCustomizado() : base(p=>CoverterParaOBancoDeDados(p),
                                            value => CoverterParaAplicacao(value),
                                            new ConverterMappingHints(1))
        {
        }

        static string CoverterParaOBancoDeDados(Status status){
            return status.ToString()[0..1];
        }

        static Status CoverterParaAplicacao(string value){
            var status = Enum.GetValues<Status>().FirstOrDefault(p=>p.ToString()[0..1] == value);

            return status;
        }
    }
}