using System;

namespace LouzadaGranitos.Core.Models
{
    public class Chapa
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public int IdBloco { get; set; }
        public string TipoGranito { get; set; } = string.Empty;
        public decimal Comprimento { get; set; }
        public decimal Largura { get; set; }
        public decimal Espessura { get; set; }
        public decimal Area { get; set; }
        public DateTime DataSerragem { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Preco { get; set; }
    }
}