using System;

namespace LouzadaGranitos.Core.Models
{
    public class Bloco
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public int IdPedreira { get; set; }
        public string TipoGranito { get; set; } = string.Empty;
        public decimal Comprimento { get; set; }
        public decimal Largura { get; set; }
        public decimal Altura { get; set; }
        public decimal Peso { get; set; }
        public DateTime DataExtracao { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Preco { get; set; }
    }
}