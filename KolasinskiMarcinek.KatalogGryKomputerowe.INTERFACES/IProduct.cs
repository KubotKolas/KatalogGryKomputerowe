namespace KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;

public interface IProduct
{
    int Id { get; set; }
    string Name { get; set; }
    IProducer Producer { get; set; }
}
