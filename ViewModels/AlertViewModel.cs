using System.ComponentModel.DataAnnotations;

namespace ProjetoESG.ViewModels
{
    public class AlertResponseViewModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public double ConsumptionKwh { get; set; }
        public string Severity { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool IsResolved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public int? EnergyConsumptionId { get; set; }
    }

    public class PaginatedAlertViewModel
    {
        public List<AlertResponseViewModel> Data { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

    public class CheckAlertsResponseViewModel
    {
        public int GeneratedAlertsCount { get; set; }
        public List<AlertResponseViewModel> NewAlerts { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }
} 