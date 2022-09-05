using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curso.Configurations
{
    public class EstadoConfiguration : IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> builder)
        {
            builder
            .HasOne(p=> p.Governador)
            .WithOne(p=>p.Estado)
            .HasForeignKey<Governador>(p=>p.EstadoId);

            builder.Navigation(p=>p.Governador).AutoInclude();

            builder
            .HasMany(p=> p.Cidades)
            .WithOne(p=>p.Estado);
            // .IsRequired(false) esse faz com que eu possa inserir uma cidade sem ter um estado(FK)
            // .OnDelete(DeleteBehavior.Restrict) para não deletar em cascata
        }
    }
}