using System.ComponentModel.DataAnnotations;

namespace ProjetoESG.Models
{
    public class Alert
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da empresa é obrigatório")]
        [StringLength(200, ErrorMessage = "O nome da empresa deve ter no máximo 200 caracteres")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "A mensagem do alerta é obrigatória")]
        [StringLength(1000, ErrorMessage = "A mensagem deve ter no máximo 1000 caracteres")]
        public string Message { get; set; } = string.Empty;

        [Required(ErrorMessage = "O consumo que gerou o alerta é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O consumo deve ser maior que zero")]
        public double ConsumptionKwh { get; set; }

        [Required(ErrorMessage = "O tipo de severidade é obrigatório")]
        [StringLength(50, ErrorMessage = "A severidade deve ter no máximo 50 caracteres")]
        public string Severity { get; set; } = "High"; // Low, Medium, High, Critical

        [Required(ErrorMessage = "A localização é obrigatória")]
        [StringLength(300, ErrorMessage = "A localização deve ter no máximo 300 caracteres")]
        public string Location { get; set; } = string.Empty;

        public bool IsResolved { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ResolvedAt { get; set; }

        // Foreign Key para EnergyConsumption (opcional)
        public int? EnergyConsumptionId { get; set; }
        public EnergyConsumption? EnergyConsumption { get; set; }
    }
} 