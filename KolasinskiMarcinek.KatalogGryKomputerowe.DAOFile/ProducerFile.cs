using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.DAOFile;

public class ProducerFile : IProducer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Address { get; set; } = "";
}
