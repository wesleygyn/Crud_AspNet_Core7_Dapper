namespace Crud_AspNet_Core7_Dapper.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public DateTime Nascimento { get; set; }
        public string Telefone { get; set; }
    }
}
