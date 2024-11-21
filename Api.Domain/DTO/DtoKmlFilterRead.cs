using System.ComponentModel.DataAnnotations;

namespace Api.Domain.DTO;

public class DtoKmlFilterRead
{
    // Validação para CLIENTE
    public string Cliente { get; set; }

    // Validação para SITUAÇÃO

    public string Situacao { get; set; }

    // Validação para BAIRRO

    public string Bairro { get; set; }

    // Validação para REFERÊNCIA

    public string Referencia { get; set; }

    // Validação para RUA/CRUZAMENTO

    public string RuaCruzamento { get; set; }

    public DtoKmlFilterRead()
    {
            
    }

}
