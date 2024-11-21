using Api.Domain.DTO;
using Api.Domain.Entities;
using System.Xml.Linq;

namespace Api.Domain.Interfaces;

public interface IKmlServices
{
    List<DtoKmlFilterWrite> ObterFiltrosDisponiveis(XDocument document);

    XDocument LoadKmlFile(string diretorioLeitura);

    List<ElementoFiltrado> FilterPlacemarks(XDocument document, DtoKmlFilterRead filter);

    string GenerateKml(List<ElementoFiltrado> placemarks);

    void GravarArquivoKml(string arquivo, string diretorioescrita);
}
