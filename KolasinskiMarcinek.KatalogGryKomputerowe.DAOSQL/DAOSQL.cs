using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.DAOSQL;

public class DAOSQL : DbContext, IDAO
{
    public DbSet<GameDb> games { get; set; }
    public DbSet<ProducerDb> producers { get; set; }
    public string DbPath { get; }

    private IConfiguration _configuration;

    public DAOSQL(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DAOSQL()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join("", "game_catalog.db");
    }

    public DAOSQL(string dbFilePath)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(dbFilePath);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;


        // TODO: replace
        string projectRootDirectory = Directory
            .GetParent(currentDirectory)
            ?.Parent?.Parent?.Parent?.FullName;

        if (projectRootDirectory != null)
        {
            string dbFilePath = Path.Combine(projectRootDirectory, "game_catalog.db");
            options.UseSqlite($"Data Source={dbFilePath}");
        }
        else
        {
            throw new InvalidOperationException(
                "Failed to determine the project's root directory."
            );
        }
    }

    public IGame CreateNewGame(IGame game)
    {
        Add(new GameDb(){
            Id = game.Id,
            Name = game.Name,
            producerId = game.producer.Id,
            ReleaseYear = game.ReleaseYear,
            Multiplayer = game.Multiplayer,
            Genre = game.Genre
        });
        SaveChanges();
        return game;
    }

    public IProducer CreateNewProducecr(IProducer producer)
    {
        Add(new ProducerDb(){
            Id = producer.Id,
            Name = producer.Name,
            Address = producer.Address
        });
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

    public IEnumerable<IProducer> GetAllProducer()
    {
        return producers.Select(p => p);
    }

    public void UpdateGame(IGame game)
    {
        var newGame = games.FirstOrDefault(g => g.Id.Equals(game.Id));

        newGame.Name = game.Name;
        newGame.producerId = game.producer.Id;
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
