using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Domain.Abstract;

namespace src.Domain
{
    public class Product : BaseEntity
    {
        public string Description { get; set; }
    }
}