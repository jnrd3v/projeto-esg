using ProjetoESG.Models;
using ProjetoESG.ViewModels;

namespace ProjetoESG.Services
{
    public interface IAlertService
    {
        Task<PaginatedAlertViewModel> GetAlertsAsync(int pageNumber, int pageSize);
        Task<CheckAlertsResponseViewModel> CheckAndGenerateAlertsAsync();
        Task CreateAlertForHighConsumptionAsync(EnergyConsumption energyConsumption);
    }
} 