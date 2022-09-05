using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Curso.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cliente> builder)
        {
            builder.OwnsOne(x=> x.Endereco, end =>{
                end.Property(p=> p.Bairro).HasColumnName("Bairro");
                end.ToTable("Enderecos");
            });
        }
    }
}