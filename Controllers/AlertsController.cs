using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoESG.Services;
using ProjetoESG.ViewModels;

namespace ProjetoESG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlertsController : ControllerBase
    {
        private readonly IAlertService _alertService;
        private readonly ILogger<AlertsController> _logger;

        public AlertsController(IAlertService alertService, ILogger<AlertsController> logger)
        {
            _alertService = alertService;
            _logger = logger;
        }

        /// <summary>
        /// Lista os alertas de consumo excessivo com paginação
        /// </summary>
        /// <param name="pageNumber">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Tamanho da página (padrão: 10, máximo: 100)</param>
        /// <returns>Lista paginada de alertas</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedAlertViewModel), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PaginatedAlertViewModel>> GetAlerts(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1)
            {
                return BadRequest("O número da página deve ser maior que zero");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest("O tamanho da página deve estar entre 1 e 100");
            }

            _logger.LogInformation("🚨 [ALERTS] Listando alertas - Página {PageNumber}/{PageSize}", pageNumber, pageSize);

            var result = await _alertService.GetAlertsAsync(pageNumber, pageSize);
            
            _logger.LogInformation("✅ [ALERTS] Encontrados {TotalCount} alertas", result.TotalCount);
            return Ok(result);
        }

        /// <summary>
        /// Simula uma checagem de sensores IoT e gera alertas automaticamente
        /// </summary>
        /// <returns>Resultado da verificação e alertas gerados</returns>
        [HttpPost("check")]
        [Authorize]
        [ProducesResponseType(typeof(CheckAlertsResponseViewModel), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<CheckAlertsResponseViewModel>> CheckAlerts()
        {
            _logger.LogInformation("🤖 [IOT] Iniciando simulação de sensores IoT");

            var result = await _alertService.CheckAndGenerateAlertsAsync();
            
            if (result.GeneratedAlertsCount > 0)
            {
                _logger.LogWarning("⚠️ [IOT] Novos alertas gerados: {Count}", result.GeneratedAlertsCount);
            }
            else
            {
                _logger.LogInformation("✅ [IOT] Verificação concluída - Nenhum alerta gerado");
            }

            return Ok(result);
        }
    }
} 