using KolasinskiMarcinek.KatalogGryKomputerowe.CORE;
using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.DAOMock;

public class Game : IGame
{
    public int ReleaseYear { get; set; }
    public bool Multiplayer { get; set; }
    public GameGenre Genre { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public IProducer Producer { get; set; }
}
