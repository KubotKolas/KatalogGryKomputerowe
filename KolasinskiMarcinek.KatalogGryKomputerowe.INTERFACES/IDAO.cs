namespace KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;

public interface IDAO
{
    IEnumerable<IProducer> GetAllProducers();
    IEnumerable<IGame> GetAllGames();

    IProducer CreateNewProducer(IProducer producer);
    IGame CreateNewGame(IGame game);

    void DeleteProducer(int producerId);
    void DeleteGame(int gameId);

    void UpdateProducer(IProducer producer);
    void UpdateGame(IGame game);
}
