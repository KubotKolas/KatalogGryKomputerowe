using System.Text.Json;
using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;
using Microsoft.Extensions.Configuration;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.DAOFile;

public class DAOFile : IDAO
{
    private readonly string _gamesPath = "games.json";
    private readonly string _producersPath = "producers.json";

    public DAOFile(IConfiguration configuration) { }

    public DAOFile() { }

    private List<T> Load<T>(string path)
        where T : new()
    {
        if (!File.Exists(path))
            return new List<T>();
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }

    private void Save<T>(List<T> data, string path)
    {
        var json = JsonSerializer.Serialize(
            data,
            new JsonSerializerOptions { WriteIndented = true }
        );
        File.WriteAllText(path, json);
    }

    public IEnumerable<IProducer> GetAllProducers() => Load<ProducerFile>(_producersPath);

    public IEnumerable<IGame> GetAllGames()
    {
        var games = Load<GameFile>(_gamesPath);
        var producers = GetAllProducers().ToList();
        foreach (var game in games)
        {
            game.Producer = producers.FirstOrDefault(p => p.Id == game.ProducerId)!;
        }
        return games;
    }

    public IProducer CreateNewProducer(IProducer producer)
    {
        var data = Load<ProducerFile>(_producersPath);
        producer.Id = data.Any() ? data.Max(p => p.Id) + 1 : 1;
        data.Add(
            new ProducerFile
            {
                Id = producer.Id,
                Name = producer.Name,
                Address = producer.Address,
            }
        );
        Save(data, _producersPath);
        return producer;
    }

    public IGame CreateNewGame(IGame game)
    {
        var data = Load<GameFile>(_gamesPath);
        game.Id = data.Any() ? data.Max(g => g.Id) + 1 : 1;
        data.Add(
            new GameFile
            {
                Id = game.Id,
                Name = game.Name,
                ProducerId = game.Producer.Id,
                ReleaseYear = game.ReleaseYear,
                Genre = game.Genre,
                Multiplayer = game.Multiplayer,
            }
        );
        Save(data, _gamesPath);
        return game;
    }

    public void DeleteProducer(int id)
    {
        var data = Load<ProducerFile>(_producersPath);
        data.RemoveAll(p => p.Id == id);
        Save(data, _producersPath);
    }

    public void DeleteGame(int id)
    {
        var data = Load<GameFile>(_gamesPath);
        data.RemoveAll(g => g.Id == id);
        Save(data, _gamesPath);
    }

    public void UpdateProducer(IProducer producer)
    {
        var data = Load<ProducerFile>(_producersPath);
        var idx = data.FindIndex(p => p.Id == producer.Id);
        if (idx != -1)
        {
            data[idx] = new ProducerFile
            {
                Id = producer.Id,
                Name = producer.Name,
                Address = producer.Address,
            };
            Save(data, _producersPath);
        }
    }

    public void UpdateGame(IGame game)
    {
        var data = Load<GameFile>(_gamesPath);
        var idx = data.FindIndex(g => g.Id == game.Id);
        if (idx != -1)
        {
            data[idx] = new GameFile
            {
                Id = game.Id,
                Name = game.Name,
                ProducerId = game.Producer.Id,
                ReleaseYear = game.ReleaseYear,
                Genre = game.Genre,
                Multiplayer = game.Multiplayer,
            };
            Save(data, _gamesPath);
        }
    }
}
