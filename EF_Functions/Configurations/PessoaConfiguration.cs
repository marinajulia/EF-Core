using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curso.Configurations
{
    public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
    {

        //configuração para criar uma tabela só em formato de hierarquia TPH
        // public void Configure(EntityTypeBuilder<Pessoa> builder)
        // {
        //     builder.ToTable("Pessoas")
        //     .HasDiscriminator<int>("TipoPessoa")
        //     .HasValue<Pessoa>(3)
        //     .HasValue<Instrutor>(1)
        //     .HasValue<Aluno>(2);
        // }

        //para gerar tabelas separadas TPT
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("Pessoas");
        }
        
    }

    public class InstrutorConfiguration : IEntityTypeConfiguration<Instrutor>
    {

        public void Configure(EntityTypeBuilder<Instrutor> builder)
        {
            builder.ToTable("Instrutores");
        }
        
    }

    public class AlunoConfiguration : IEntityTypeConfiguration<Aluno>
    {

        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.ToTable("Alunos");
        }
        
    }
}