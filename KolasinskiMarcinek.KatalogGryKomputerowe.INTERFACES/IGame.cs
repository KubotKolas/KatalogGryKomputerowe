using KolasinskiMarcinek.KatalogGryKomputerowe.CORE;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES
{
    public interface IGame : IProduct
    {
        int ReleaseYear { get; set; }
        bool Multiplayer { get; set; }
        GameGenre Genre { get; set; }
    }
}
