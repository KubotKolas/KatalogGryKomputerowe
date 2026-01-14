using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.BL;

public class Producer : IProducer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}