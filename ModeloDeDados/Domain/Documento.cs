namespace Curso.Domain
{
    public class Documento
    {
        private string _cpf;
        public int Id { get; set; }
        public void SetCPF(string cpf) { 
            if(string.IsNullOrEmpty(cpf)){
                throw new System.Exception("CPF inválido");
            }
            _cpf= cpf;
        }
        public string CPF => _cpf;
    }
}