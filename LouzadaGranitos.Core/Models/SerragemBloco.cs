using System;

namespace LouzadaGranitos.Core.Models
{
    public class SerragemBloco
    {
        public int Id { get; set; }
        public int IdBloco { get; set; }
        public DateTime DataSerragem { get; set; }
        public string TipoSerragem { get; set; } = string.Empty;
        public decimal EspessuraChapa { get; set; }
        public int QuantidadeChapas { get; set; }
        public decimal Rendimento { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public string Operador { get; set; } = string.Empty;
    }
}