using System.Net.Sockets;
using System.Reflection;
using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;
using Microsoft.Extensions.Configuration;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.BL;

public class BusinessLogic
{
    private IDAO _dao;
    private static BusinessLogic instance;
    private static readonly object lockObject = new object();

    public BusinessLogic(IConfiguration configuration)
    {
        // Pobieramy nazwę biblioteki z pliku konfiguracyjnego (wymóg 2.5)
        // Szuka w appsettings.json (IConfiguration) lub App.config (ConfigurationManager)
        string libraryName =
            configuration["DAOLibraryName"]
            ?? System.Configuration.ConfigurationManager.AppSettings["DAOLibraryName"]!;

        if (string.IsNullOrEmpty(libraryName))
            throw new Exception("Brak klucza 'DAOLibraryName' w pliku konfiguracyjnym.");

        LoadLibrary(libraryName, configuration);
    }

    private void LoadLibrary(string libraryName, IConfiguration configuration)
    {
        try
        {
            // 1. Próbujemy znaleźć pełną ścieżkę do pliku DLL
            string? dllPath = FindDllPath(libraryName);

            if (dllPath == null)
                throw new FileNotFoundException(
                    $"Nie można odnaleźć pliku biblioteki: {libraryName} w żadnej znanej lokalizacji."
                );

            // 2. Ładowanie zestawu (assembly) z pełnej ścieżki
            Assembly assembly = Assembly.UnsafeLoadFrom(dllPath);

            // Szukanie typu implementującego IDAO
            Type? daoType = assembly
                .GetTypes()
                .FirstOrDefault(t =>
                    typeof(IDAO).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract
                );

            if (daoType == null)
                throw new Exception(
                    $"W bibliotece {dllPath} nie znaleziono klasy implementującej IDAO."
                );

            ConstructorInfo? constructor = daoType.GetConstructor(new[] { typeof(IConfiguration) });

            if (constructor != null)
                _dao = (IDAO)constructor.Invoke(new object[] { configuration });
            else
                _dao = (IDAO)Activator.CreateInstance(daoType)!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Błąd Late Binding (DAO) dla {libraryName}: {ex.Message}");
        }
    }

    private string? FindDllPath(string libraryName)
    {
        // 1. Sprawdź folder roboczy (tam gdzie jest .exe)
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string localPath = Path.Combine(baseDir, libraryName);
        if (File.Exists(localPath))
            return localPath;

        // 2. Jeśli nie ma, szukaj w folderach nadrzędnych (do 4 poziomów w górę)
        DirectoryInfo? currentDir = new DirectoryInfo(baseDir);

        for (int i = 0; i < 4; i++)
        {
            if (currentDir == null)
                break;

            // Szukaj pliku rekurencyjnie w aktualnym folderze (np. w całym Solution)
            // Ograniczamy się do folderów "bin", żeby nie skanować wszystkiego
            var files = currentDir.GetFiles(libraryName, SearchOption.AllDirectories);
            var bestMatch = files.OrderByDescending(f => f.LastWriteTime).FirstOrDefault();

            if (bestMatch != null)
                return bestMatch.FullName;

            currentDir = currentDir.Parent;
        }

        return null;
    }

    public static BusinessLogic GetInstance(IConfiguration configuration)
    {
        if (instance == null)
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = new BusinessLogic(configuration);
                }
            }
        }
        return instance;
    }

    public IEnumerable<IProducer> GetAllProducers() => _dao.GetAllProducers();

    public IEnumerable<IGame> GetAllGames() => _dao.GetAllGames();

    public IProducer CreateNewProducer(IProducer producer) => _dao.CreateNewProducer(producer);

    public IGame CreateNewGame(IGame game) => _dao.CreateNewGame(game);

    public void DeleteProducer(int producerId) => _dao.DeleteProducer(producerId);

    public void DeleteGame(int gameId) => _dao.DeleteGame(gameId);

    public void UpdateProducer(IProducer producer) => _dao.UpdateProducer(producer);

    public void UpdateGame(IGame game) => _dao.UpdateGame(game);
}
