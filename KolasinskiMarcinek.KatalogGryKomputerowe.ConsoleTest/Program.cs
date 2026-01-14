using KolasinskiMarcinek.KatalogGryKomputerowe.BL;
using KolasinskiMarcinek.KatalogGryKomputerowe.CORE;
using Microsoft.Extensions.Configuration;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.ConsoleTest;

class Program
{
    static void Main(string[] args)
    {
        string[] libraries =
        {
            "KolasinskiMarcinek.KatalogGryKomputerowe.DAOMock.dll",
            "KolasinskiMarcinek.KatalogGryKomputerowe.DAOFile.dll",
            "KolasinskiMarcinek.KatalogGryKomputerowe.DAOSQL.dll",
        };

        foreach (var lib in libraries)
        {
            Console.WriteLine($"\n=== TESTOWANIE BIBLIOTEKI: {lib} ===");
            try
            {
                var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(
                        new Dictionary<string, string?> { { "DAOLibraryName", lib } }
                    )
                    .Build();
                var bl = new BusinessLogic(config);
                RunFullTest(bl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BŁĄD DLA {lib}: {ex.Message}");
            }
        }
    }

    static void RunFullTest(BusinessLogic bl)
    {
        // 1. CREATE Producer
        Console.WriteLine("-> Tworzenie producenta...");
        var p = bl.CreateNewProducer(new Producer { Name = "Test Producer", Address = "Test Ave" });

        // 2. CREATE Game
        Console.WriteLine("-> Tworzenie gry...");
        var g = bl.CreateNewGame(
            new Game
            {
                Name = "Test Game",
                Producer = p,
                ReleaseYear = 2024,
                Genre = GameGenre.Action,
                Multiplayer = true,
            }
        );

        // 3. READ
        Console.WriteLine($"-> Wczytano gier: {bl.GetAllGames().Count()}");
        Console.WriteLine(
            $"-> Ostatnia gra: {bl.GetAllGames().Last().Name} by {bl.GetAllGames().Last().Producer.Name}"
        );

        // 4. UPDATE
        Console.WriteLine("-> Aktualizacja nazwy gry...");
        g.Name = "Updated Game Name";
        bl.UpdateGame(g);
        Console.WriteLine($"-> Po aktualizacji: {bl.GetAllGames().Last().Name}");

        // 5. DELETE
        Console.WriteLine("-> Usuwanie gry i producenta...");
        bl.DeleteGame(g.Id);
        bl.DeleteProducer(p.Id);

        Console.WriteLine("=== TEST ZAKOŃCZONY SUKCESEM ===");
    }
}
