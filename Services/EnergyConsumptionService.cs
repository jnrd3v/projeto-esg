using Microsoft.EntityFrameworkCore;
using ProjetoESG.Data;
using ProjetoESG.Models;
using ProjetoESG.ViewModels;

namespace ProjetoESG.Services
{
    public class EnergyConsumptionService : IEnergyConsumptionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAlertService _alertService;

        public EnergyConsumptionService(ApplicationDbContext context, IAlertService alertService)
        {
            _context = context;
            _alertService = alertService;
        }

        public async Task<PaginatedEnergyConsumptionViewModel> GetEnergyConsumptionsAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.EnergyConsumptions.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var consumptions = await _context.EnergyConsumptions
                .OrderByDescending(x => x.Timestamp)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new EnergyConsumptionResponseViewModel
                {
                    Id = x.Id,
                    CompanyName = x.CompanyName,
                    ConsumptionKwh = x.ConsumptionKwh,
                    Timestamp = x.Timestamp,
                    Description = x.Description,
                    Location = x.Location,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return new PaginatedEnergyConsumptionViewModel
            {
                Data = consumptions,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        public async Task<EnergyConsumptionResponseViewModel> CreateEnergyConsumptionAsync(CreateEnergyConsumptionViewModel model)
        {
            var energyConsumption = new EnergyConsumption
            {
                CompanyName = model.CompanyName,
                ConsumptionKwh = model.ConsumptionKwh,
                Timestamp = model.Timestamp,
                Description = model.Description,
                Location = model.Location,
                CreatedAt = DateTime.UtcNow
            };

            _context.EnergyConsumptions.Add(energyConsumption);
            await _context.SaveChangesAsync();

            // Verificar se deve gerar alerta (consumo > 1000 kWh)
            if (energyConsumption.ConsumptionKwh > 1000)
            {
                await _alertService.CreateAlertForHighConsumptionAsync(energyConsumption);
            }

            return new EnergyConsumptionResponseViewModel
            {
                Id = energyConsumption.Id,
                CompanyName = energyConsumption.CompanyName,
                ConsumptionKwh = energyConsumption.ConsumptionKwh,
                Timestamp = energyConsumption.Timestamp,
                Description = energyConsumption.Description,
                Location = energyConsumption.Location,
                CreatedAt = energyConsumption.CreatedAt
            };
        }

        public async Task<EnergyConsumption?> GetEnergyConsumptionByIdAsync(int id)
        {
            return await _context.EnergyConsumptions.FindAsync(id);
        }
    }
} 