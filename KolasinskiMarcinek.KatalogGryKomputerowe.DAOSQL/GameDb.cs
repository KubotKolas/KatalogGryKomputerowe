using System.ComponentModel.DataAnnotations;
using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;
using KolasinskiMarcinek.KatalogGryKomputerowe.CORE;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.DAOSQL;

public class GameDb
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int producerId { get; set; }
    public int ReleaseYear { get; set; }
    public bool Multiplayer { get; set; }
    public GameGenre Genre { get; set; }

    public IGame ToIGame(List<ProducerDb> producers){
        var foundProducer = producers.FirstOrDefault(p => p.Id == producerId);

        return new Game(){
            Id = Id,
            Name = Name,
            Producer = foundProducer?.ToIProducer() ?? new ProducerDb.Producer { Name = "Nieznany" },
            ReleaseYear = ReleaseYear,
            Multiplayer = Multiplayer,
            Genre = Genre
        };
    }

    public class Game : IGame
    {
        public int ReleaseYear { get; set; }
        public bool Multiplayer { get; set; }
        public GameGenre Genre { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public IProducer Producer { get; set; }
    }
}
