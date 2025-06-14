using ProjetoESG.Models;
using ProjetoESG.ViewModels;

namespace ProjetoESG.Services
{
    public interface IEnergyConsumptionService
    {
        Task<PaginatedEnergyConsumptionViewModel> GetEnergyConsumptionsAsync(int pageNumber, int pageSize);
        Task<EnergyConsumptionResponseViewModel> CreateEnergyConsumptionAsync(CreateEnergyConsumptionViewModel model);
        Task<EnergyConsumption?> GetEnergyConsumptionByIdAsync(int id);
    }
} 