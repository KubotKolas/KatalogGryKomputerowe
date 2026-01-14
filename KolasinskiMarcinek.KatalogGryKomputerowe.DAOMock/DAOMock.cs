using System;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using KolasinskiMarcinek.KatalogGryKomputerowe.CORE;
using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.DAOMock;

public class DAOMock : IDAO
{
    private List<IProducer> producers;
    private List<IGame> games;
    private int nextIdGame = 3;
    private int nextIdProd = 3;

    public DAOMock()
    {
        producers = new List<IProducer>()
        {
            new Producer()
            {
                Id = 1,
                Name = "Bethesda",
                Address = "Placeholder Address 1",
            },
            new Producer()
            {
                Id = 2,
                Name = "Larian",
                Address = "Placeholder Address 2",
            },
        };

        games = new List<IGame>()
        {
            new Game()
            {
                Id = 1,
                Name = "Baldure's Gate 3",
                Producer = producers[1],
                ReleaseYear = 2023,
                Multiplayer = true,
                Genre = GameGenre.RPG,
            },
            new Game()
            {
                Id = 2,
                Name = "Starfield",
                Producer = producers[0],
                ReleaseYear = 2023,
                Multiplayer = false,
                Genre = GameGenre.Adventure,
            },
        };
    }

    public IProducer CreateNewProducer(IProducer producer)
    {
        producer.Id = nextIdProd++;
        producers.Add(producer);
        return producer;
    }

    public IGame CreateNewGame(IGame game)
    {
        game.Id = nextIdGame++;
        games.Add(game);
        return game;
    }

    public void DeleteProducer(int producerId)
    {
        IProducer prodToDelete = producers.First(p => p.Id.Equals(producerId));
        producers.Remove(prodToDelete);
    }

    public void DeleteGame(int gameId)
    {
        IGame gameToDelete = games.First(g => g.Id.Equals(gameId));
        games.Remove(gameToDelete);
    }

    public void UpdateProducer(IProducer producer)
    {
        int prodToUpdate = producers.FindIndex(old => old.Id.Equals(producer.Id));
        if (prodToUpdate != -1)
        {
            producers[prodToUpdate] = producer;
        }
        else
        {
            CreateNewProducer(producer);
        }
    }

    public void UpdateGame(IGame game)
    {
        int gameToUpdate = games.FindIndex(old => old.Id.Equals(game.Id));
        if (gameToUpdate != -1)
        {
            games[gameToUpdate] = game;
        }
        else
        {
            CreateNewGame(game);
        }
    }

    public IEnumerable<IProducer> GetAllProducers()
    {
        return producers;
    }

    public IEnumerable<IGame> GetAllGames()
    {
        return games;
    }
}
