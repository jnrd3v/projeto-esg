using Microsoft.EntityFrameworkCore;
using ProjetoESG.Data;
using ProjetoESG.Models;
using ProjetoESG.ViewModels;

namespace ProjetoESG.Services
{
    public class AlertService : IAlertService
    {
        private readonly ApplicationDbContext _context;

        public AlertService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedAlertViewModel> GetAlertsAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Alerts.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var alerts = await _context.Alerts
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new AlertResponseViewModel
                {
                    Id = x.Id,
                    CompanyName = x.CompanyName,
                    Message = x.Message,
                    ConsumptionKwh = x.ConsumptionKwh,
                    Severity = x.Severity,
                    Location = x.Location,
                    IsResolved = x.IsResolved,
                    CreatedAt = x.CreatedAt,
                    ResolvedAt = x.ResolvedAt,
                    EnergyConsumptionId = x.EnergyConsumptionId
                })
                .ToListAsync();

            return new PaginatedAlertViewModel
            {
                Data = alerts,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        public async Task<CheckAlertsResponseViewModel> CheckAndGenerateAlertsAsync()
        {
            // Simula verificação de sensores IoT - busca consumos das últimas 24 horas sem alertas
            var cutoffTime = DateTime.UtcNow.AddHours(-24);
            
            var highConsumptions = await _context.EnergyConsumptions
                .Where(ec => ec.ConsumptionKwh > 1000 && 
                           ec.CreatedAt >= cutoffTime &&
                           !_context.Alerts.Any(a => a.EnergyConsumptionId == ec.Id))
                .ToListAsync();

            var newAlerts = new List<AlertResponseViewModel>();

            foreach (var consumption in highConsumptions)
            {
                var alert = await CreateAlertForHighConsumptionAsync(consumption);
                if (alert != null)
                {
                    newAlerts.Add(new AlertResponseViewModel
                    {
                        Id = alert.Id,
                        CompanyName = alert.CompanyName,
                        Message = alert.Message,
                        ConsumptionKwh = alert.ConsumptionKwh,
                        Severity = alert.Severity,
                        Location = alert.Location,
                        IsResolved = alert.IsResolved,
                        CreatedAt = alert.CreatedAt,
                        ResolvedAt = alert.ResolvedAt,
                        EnergyConsumptionId = alert.EnergyConsumptionId
                    });
                }
            }

            return new CheckAlertsResponseViewModel
            {
                GeneratedAlertsCount = newAlerts.Count,
                NewAlerts = newAlerts,
                Message = newAlerts.Count > 0 
                    ? $"Foram gerados {newAlerts.Count} novos alertas baseados na simulação de sensores IoT."
                    : "Nenhum novo alerta foi gerado. Consumos dentro dos parâmetros normais."
            };
        }

        public async Task<Alert?> CreateAlertForHighConsumptionAsync(EnergyConsumption energyConsumption)
        {
            // Verifica se já existe um alerta para este consumo
            var existingAlert = await _context.Alerts
                .AnyAsync(a => a.EnergyConsumptionId == energyConsumption.Id);

            if (existingAlert)
                return null;

            var severity = energyConsumption.ConsumptionKwh switch
            {
                > 5000 => "Critical",
                > 3000 => "High",
                > 1500 => "Medium",
                _ => "Low"
            };

            var alert = new Alert
            {
                CompanyName = energyConsumption.CompanyName,
                Message = $"Consumo excessivo de energia detectado: {energyConsumption.ConsumptionKwh:F2} kWh. " +
                         $"Este valor está acima do limite recomendado de 1000 kWh. " +
                         $"Recomenda-se revisão imediata dos equipamentos em {energyConsumption.Location}.",
                ConsumptionKwh = energyConsumption.ConsumptionKwh,
                Severity = severity,
                Location = energyConsumption.Location,
                IsResolved = false,
                EnergyConsumptionId = energyConsumption.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Alerts.Add(alert);
            await _context.SaveChangesAsync();

            return alert;
        }

        // Método auxiliar para retornar Alert (sobrecarga)
        async Task IAlertService.CreateAlertForHighConsumptionAsync(EnergyConsumption energyConsumption)
        {
            await CreateAlertForHighConsumptionAsync(energyConsumption);
        }
    }
} 