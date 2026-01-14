namespace KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;

public interface IDAO
{
    IEnumerable<IProducer> GetAllProducer();
    IEnumerable<IGame> GetAllGames();

    IProducer CreateNewProducecr(IProducer producer);
    IGame CreateNewGame(IGame game);

    void DeleteProducer(int producerId);
    void DeleteGame(int gameId);

    void UpdateProducer(IProducer producer);
    void UpdateGame(IGame game);
}
