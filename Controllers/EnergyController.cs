using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoESG.Services;
using ProjetoESG.ViewModels;

namespace ProjetoESG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnergyController : ControllerBase
    {
        private readonly IEnergyConsumptionService _energyConsumptionService;
        private readonly ILogger<EnergyController> _logger;

        public EnergyController(IEnergyConsumptionService energyConsumptionService, ILogger<EnergyController> logger)
        {
            _energyConsumptionService = energyConsumptionService;
            _logger = logger;
        }

        /// <summary>
        /// Lista os consumos de energia registrados com paginação
        /// </summary>
        /// <param name="pageNumber">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Tamanho da página (padrão: 10, máximo: 100)</param>
        /// <returns>Lista paginada de consumos de energia</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedEnergyConsumptionViewModel), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<PaginatedEnergyConsumptionViewModel>> GetEnergyConsumptions(
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

            _logger.LogInformation("🔍 [ENERGY] Listando consumos - Página {PageNumber}/{PageSize}", pageNumber, pageSize);

            var result = await _energyConsumptionService.GetEnergyConsumptionsAsync(pageNumber, pageSize);
            
            _logger.LogInformation("✅ [ENERGY] Encontrados {TotalCount} consumos", result.TotalCount);
            return Ok(result);
        }

        /// <summary>
        /// Registra um novo consumo de energia
        /// </summary>
        /// <param name="model">Dados do consumo de energia</param>
        /// <returns>Consumo de energia criado</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(EnergyConsumptionResponseViewModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<EnergyConsumptionResponseViewModel>> CreateEnergyConsumption(
            [FromBody] CreateEnergyConsumptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("➕ [ENERGY] Criando consumo - Empresa: {CompanyName} | {ConsumptionKwh} kWh", 
                model.CompanyName, model.ConsumptionKwh);

            var result = await _energyConsumptionService.CreateEnergyConsumptionAsync(model);
            
            _logger.LogInformation("✅ [ENERGY] Consumo criado - ID: {Id} | Alertas podem ter sido gerados", result.Id);

            return CreatedAtAction(
                nameof(GetEnergyConsumptions), 
                new { id = result.Id }, 
                result);
        }
    }
} 