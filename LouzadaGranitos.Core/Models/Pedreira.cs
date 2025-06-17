namespace LouzadaGranitos.Core.Models
{
    public class Pedreira
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Localizacao { get; set; } = string.Empty;
        public string Proprietario { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
        public bool Ativa { get; set; }
    }
}