using Api.Domain.DTO;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace Api.Services;

public class KmlServices : IKmlServices
{
    private static readonly XNamespace ns = "http://www.opengis.net/kml/2.2";

    public List<DtoKmlFilterWrite> ObterFiltrosDisponiveis(XDocument document)
    {
        XNamespace ns = "http://www.opengis.net/kml/2.2";

        return document.Descendants(ns + "Placemark")
            .Select(p => new DtoKmlFilterWrite
            {

                Bairro = p.Descendants(ns + "Data")
                          .FirstOrDefault(data => (string)data.Attribute("name") == "BAIRRO")
                          ?.Element(ns + "value")?.Value,
                Cliente = p.Descendants(ns + "Data")
                           .FirstOrDefault(data => (string)data.Attribute("name") == "CLIENTE")
                           ?.Element(ns + "value")?.Value,
                Situacao = p.Descendants(ns + "Data")
                            .FirstOrDefault(data => (string)data.Attribute("name") == "SITUAÇÃO")
                            ?.Element(ns + "value")?.Value
            })
            .Distinct().ToList();

    }

    public XDocument LoadKmlFile(string diretorioLeitura)
    {
        if (!System.IO.File.Exists(diretorioLeitura))
            throw new FileNotFoundException("Arquivo KML não encontrado.");

        var kmlData = System.IO.File.ReadAllText(diretorioLeitura);
        return XDocument.Parse(kmlData);
    }

    public List<ElementoFiltrado> FilterPlacemarks(XDocument document, DtoKmlFilterRead filter)
    {
        XNamespace ns = "http://www.opengis.net/kml/2.2";

        var placemarks = document.Descendants(ns + "Placemark")
            .Select(p => new ElementoFiltrado
            {
                Name = p.Element(ns + "name")?.Value,
                Cliente = p.Descendants(ns + "Data")
                           .FirstOrDefault(d => d.Attribute("name")?.Value == "CLIENTE")
                           ?.Element(ns + "value")?.Value,
                Situacao = p.Descendants(ns + "Data")
                            .FirstOrDefault(d => d.Attribute("name")?.Value == "SITUAÇÃO")
                            ?.Element(ns + "value")?.Value,
                Bairro = p.Descendants(ns + "Data")
                          .FirstOrDefault(d => d.Attribute("name")?.Value == "BAIRRO")
                          ?.Element(ns + "value")?.Value,
                Referencia = p.Descendants(ns + "Data")
                              .FirstOrDefault(d => d.Attribute("name")?.Value == "REFERENCIA")
                              ?.Element(ns + "value")?.Value,
                RuaCruzamento = p.Descendants(ns + "Data")
                                 .FirstOrDefault(d => d.Attribute("name")?.Value == "RUA/CRUZAMENTO")
                                 ?.Element(ns + "value")?.Value,
                Description = p.Element(ns + "description")?.Value
            })

            .Where(p =>
                (string.IsNullOrEmpty(filter.Cliente) || p.Cliente == filter.Cliente) &&
                (string.IsNullOrEmpty(filter.Situacao) || p.Situacao == filter.Situacao) &&
                (string.IsNullOrEmpty(filter.Bairro) || p.Bairro == filter.Bairro) &&
                (string.IsNullOrEmpty(filter.Referencia) ||
                 (filter.Referencia.Length >= 3 && p.Referencia?.Contains(filter.Referencia, StringComparison.OrdinalIgnoreCase) == true)) &&
                (string.IsNullOrEmpty(filter.RuaCruzamento) ||
                 (filter.RuaCruzamento.Length >= 3 && p.RuaCruzamento?.Contains(filter.RuaCruzamento, StringComparison.OrdinalIgnoreCase) == true))
            )
            .ToList();

        return placemarks;
    }

    public string GenerateKml(List<ElementoFiltrado> placemarks)
    {
        var kml = new XDocument(
            new XElement("kml",
                new XAttribute(XNamespace.Xmlns + "kml", "http://www.opengis.net/kml/2.2"),
                new XElement("Document",
                    placemarks.Select(p => new XElement("Placemark",
                        new XElement("name", p.Name),
                        new XElement("description",
                            new XCData(p.Description)
                        ),
                        new XElement("ExtendedData",
                            new XElement("Data",
                                new XAttribute("name", "RUA/CRUZAMENTO"),
                                new XElement("value", p.RuaCruzamento)
                            ),
                            new XElement("Data",
                                new XAttribute("name", "REFERENCIA"),
                                new XElement("value", p.Referencia)
                            ),
                            new XElement("Data",
                                new XAttribute("name", "BAIRRO"),
                                new XElement("value", p.Bairro)
                            ),
                            new XElement("Data",
                                new XAttribute("name", "SITUAÇÃO"),
                                new XElement("value", p.Situacao)
                            ),
                            new XElement("Data",
                                new XAttribute("name", "CLIENTE"),
                                new XElement("value", p.Cliente)
                            )
                        )
                    ))
                )
            )
        );

        return kml.ToString();
    }

    public void GravarArquivoKml(string arquivo, string diretorioescrita)
    {
        string caminhoArquivo = Path.Combine(diretorioescrita, $"NovoKml_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.kml");
        System.IO.File.WriteAllText(caminhoArquivo, arquivo, Encoding.UTF8);
    }

}

