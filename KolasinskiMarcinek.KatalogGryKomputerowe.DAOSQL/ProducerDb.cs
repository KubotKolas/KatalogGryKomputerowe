using System.ComponentModel.DataAnnotations;
using KolasinskiMarcinek.KatalogGryKomputerowe.INTERFACES;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.DAOSQL;

public class ProducerDb : IProducer
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }

    public IProducer ToIProducer()
    {
        return new Producer()
        {
            Id = Id,
            Name = Name,
            Address = Address,
        };
    }

    public class Producer : IProducer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
