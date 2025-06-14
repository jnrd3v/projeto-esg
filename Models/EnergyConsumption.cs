using System.ComponentModel.DataAnnotations;

namespace ProjetoESG.Models
{
    public class EnergyConsumption
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da empresa é obrigatório")]
        [StringLength(200, ErrorMessage = "O nome da empresa deve ter no máximo 200 caracteres")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O consumo em kWh é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O consumo deve ser maior que zero")]
        public double ConsumptionKwh { get; set; }

        [Required(ErrorMessage = "A data e hora são obrigatórias")]
        public DateTime Timestamp { get; set; }

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "A localização é obrigatória")]
        [StringLength(300, ErrorMessage = "A localização deve ter no máximo 300 caracteres")]
        public string Location { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
} 