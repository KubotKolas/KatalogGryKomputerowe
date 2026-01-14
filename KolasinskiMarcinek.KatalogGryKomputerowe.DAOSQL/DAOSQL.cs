using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.DAOSQL;

public class DAOSQL : DbContext, IDAO
{
    public DbSet<GameDb> games { get; set; }
    public DbSet<ProducerDb> producers { get; set; }
    public string DbPath { get; }

    private readonly IConfiguration? _configuration;

    public DAOSQL(IConfiguration configuration)
    {
        _configuration = configuration;
        Database.EnsureCreated();

        if (!producers.Any())
        {
            SeedData();
        }
    }

    public DAOSQL() { }

    private void SeedData()
    {
        var p1 = new ProducerDb { Name = "Bethesda", Address = "USA" };
        producers.Add(p1);

        games.Add(
            new GameDb
            {
                Name = "Skyrim",
                producerId = p1.Id,
                ReleaseYear = 2011,
                Genre = CORE.GameGenre.RPG,
            }
        );

        SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        string? connectionString = _configuration?.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = Path.Combine(baseDir, "game_catalog.db");
            connectionString = $"Data Source={dbPath}";
        }

        options.UseSqlite(connectionString);
    }

    public IGame CreateNewGame(IGame game)
    {
        Add(
            new GameDb()
            {
                Id = game.Id,
                Name = game.Name,
                producerId = game.Producer.Id,
                ReleaseYear = game.ReleaseYear,
                Multiplayer = game.Multiplayer,
                Genre = game.Genre,
            }
        );
        SaveChanges();
        return game;
    }

    public IProducer CreateNewProducer(IProducer producer)
    {
        Add(
            new ProducerDb()
            {
                Id = producer.Id,
                Name = producer.Name,
                Address = producer.Address,
            }
        );
        SaveChanges();
        return producer;
    }

    public void DeleteGame(int gameId)
    {
        var gameToDelete = games.FirstOrDefault(g => g.Id.Equals(gameId));
        Remove(gameToDelete);
        SaveChanges();
    }

    public void DeleteProducer(int producerId)
    {
        var prodToDelete = producers.FirstOrDefault(p => p.Id.Equals(producerId));
        Remove(prodToDelete);
        SaveChanges();
    }

    // TODO: doublecheck
    public IEnumerable<IGame> GetAllGames()
    {
        return games.Select(g => g.ToIGame(producers.ToList()));
    }

    public IEnumerable<IProducer> GetAllProducers()
    {
        return producers.Select(p => p);
    }

    public void UpdateGame(IGame game)
    {
        var newGame = games.FirstOrDefault(g => g.Id.Equals(game.Id));

        newGame.Name = game.Name;
        newGame.producerId = game.Producer.Id;
        newGame.ReleaseYear = game.ReleaseYear;
        newGame.Multiplayer = game.Multiplayer;
        newGame.Genre = game.Genre;

        Entry(newGame).CurrentValues.SetValues(newGame);
        SaveChanges();
    }

    public void UpdateProducer(IProducer producer)
    {
        var newProducer = producers.FirstOrDefault(p => p.Id.Equals(producer.Id));

        newProducer.Name = producer.Name;
        newProducer.Address = producer.Address;

        Entry(newProducer).CurrentValues.SetValues(newProducer);
        SaveChanges();
    }
}
