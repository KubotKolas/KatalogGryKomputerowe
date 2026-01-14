using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.DAOFile;

public class GameFile : IGame
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int ProducerId { get; set; }
    public int ReleaseYear { get; set; }
    public bool Multiplayer { get; set; }
    public CORE.GameGenre Genre { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public IProducer Producer { get; set; } = null!;
}
