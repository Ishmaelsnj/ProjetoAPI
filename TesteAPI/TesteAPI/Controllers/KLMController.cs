using Api.Domain.DTO;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Api.Domain.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Linq;

namespace Api.Web.Controllers;


[ApiController]
[Route("api/placemarks")]
//namespace TesteAPI.Controllers
//{
public class KLMController : ControllerBase
{
    private readonly IKmlServices _kmlServices;

    private readonly string _diretorioLeitura;
    private readonly string _diretorioEscrita;

    public KLMController(IKmlServices kmlServices, IConfiguration configuration = null)
    {
        _kmlServices = kmlServices;
        _diretorioLeitura = configuration["KmlConfig:DiretorioLeitura"];
        _diretorioEscrita = configuration["KmlConfig:DiretorioEscrita"];

    }

    [HttpPost("export")]
    public IActionResult ExportFilteredKml([FromBody] DtoKmlFilterRead filter)
    {
        try
        {
            var document = _kmlServices.LoadKmlFile(_diretorioLeitura);

            var filterOptions = _kmlServices.ObterFiltrosDisponiveis(document);
            var validator = new DtoKmlFilterReadValidator(filterOptions);
            var validationResult = validator.Validate(filter);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var filterPlacemarks = _kmlServices.FilterPlacemarks(document, filter);
            var newKml = _kmlServices.GenerateKml(filterPlacemarks);
            _kmlServices.GravarArquivoKml(newKml,_diretorioEscrita);
            return Ok(new { message = "Arquivo KML exportado com sucesso.", path = _diretorioEscrita });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao exportar o KML: {ex.Message}");
        }
    }

    [HttpGet]
    public IActionResult ListFilteredPlacemarks([FromQuery] DtoKmlFilterRead filter)
    {
        try
        {
            var document = _kmlServices.LoadKmlFile(_diretorioLeitura);

            var filterOptions = _kmlServices.ObterFiltrosDisponiveis(document);
            var validator = new DtoKmlFilterReadValidator(filterOptions);
            var validationResult = validator.Validate(filter);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var newKml = _kmlServices.FilterPlacemarks(document, filter);
            
            return Ok(new { message = "Arquivo KML exportado com sucesso.", path = newKml });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao exportar o KML: {ex.Message}");
        }
    }

    [HttpGet("filters")]
    public IActionResult GetFilterOptions()
    {
        try
        {
            var document = _kmlServices.LoadKmlFile(_diretorioLeitura);

            var filterOptions = _kmlServices.ObterFiltrosDisponiveis(document);

            return Ok(filterOptions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao obter os filtros: {ex.Message}");
        }
    }

}
