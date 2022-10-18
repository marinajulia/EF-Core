using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Domain
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Descrição { get; set; }

        public List<Colaborador> Colaboradores { get; set; }
    }
}