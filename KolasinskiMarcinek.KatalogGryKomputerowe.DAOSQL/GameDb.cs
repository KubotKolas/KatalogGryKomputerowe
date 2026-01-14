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
        return new Game(){
            Id = Id,
            Name = Name,
            producer = producers.Single(p => p.Id.Equals(producerId)).ToIProducer(),
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
        public IProducer producer { get; set; }
    }
}
