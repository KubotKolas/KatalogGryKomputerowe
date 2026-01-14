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
        string libraryName = System.Configuration.ConfigurationManager.AppSettings[
            "DAOLibraryName"
        ]!;
        Assembly assembly = Assembly.UnsafeLoadFrom(libraryName);
        Type? typeToCreate = null;

        foreach (Type type in assembly.GetTypes())
        {
            if (type.IsAssignableTo(typeof(IDAO)))
            {
                typeToCreate = type;
                break;
            }
        }
        ConstructorInfo? constructor = typeToCreate!.GetConstructor(
            new[] { typeof(IConfiguration) }
        );
        if (constructor != null)
        {
            _dao = (IDAO)constructor.Invoke(new object[] { configuration })!;
        }
        else
        {
            _dao = (IDAO)Activator.CreateInstance(typeToCreate!, null)!;
        }
    }

    public BusinessLogic(string path)
    {
        LoadDatasource(path);
    }

    public void LoadDatasource(string path)
    {
        if (path.EndsWith(".dll"))
            LoadLibrary(path);
        else if (path.EndsWith(".db"))
            LoadSql(path);
    }

    public void LoadSql(string path)
    {
        _dao = new DAOSQL(path);
    }

    public void LoadLibrary(string path)
    {
        var typeToCreate = FindDAOType(path);

        if (typeToCreate != null)
        {
            _dao = CreateDAOInstance(typeToCreate);
        }
        else
        {
            throw new InvalidOperationException("No compatible IDAO type found in assembly.");
        }
    }

    public static BL GetInstance(string libraryName)
    {
        if (instance == null)
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = new BL(libraryName);
                }
            }
        }

        return instance;
    }

    private Type FindDAOType(string path)
    {
        try
        {
            var assembly = Assembly.UnsafeLoadFrom(path);
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IDAO).IsAssignableFrom(type))
                {
                    return type;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to load assembly or find IDAO: " + ex.Message);
            throw;
        }

        return null;
    }

    private IDAO CreateDAOInstance(Type daoType)
    {
        try
        {
            return (IDAO)Activator.CreateInstance(daoType);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to create instance of IDAO: {daoType.Name}\n{ex.Message}");
            throw;
        }
    }

    public ICPU NewCpu()
    {
        return _dao.NewCPU();
    }

    public IEnumerable<ICPU> GetAllCPUs()
    {
        return _dao.GetAllCPUs();
    }

    public IEnumerable<IManufacturer> GetAllManufacturers()
    {
        return _dao.GetAllManufaturers();
    }

    public IManufacturer GetManufacturerById(int manId)
    {
        return _dao.GetAllManufaturers().First(m => m.Id.Equals(manId));
    }

    public ICPU GetCPUById(int cpuId)
    {
        return _dao.GetAllCPUs().First(m => m.Id.Equals(cpuId));
    }

    //GET ALL
    public IEnumerable<string> GetAllCPUsNames() => from cpu in _dao.GetAllCPUs() select cpu.Name;

    public IEnumerable<string> GetAllManufacturersNames() =>
        from manufaturer in _dao.GetAllManufaturers()
        select manufaturer.Name;

    //CREATE
    public void CreateNewCPU(ICPU cpu)
    {
        _dao.CreateCPU(cpu);
    }

    public void CreateCPU(ICPU cpu)
    {
        _dao.CreateNewCPU(cpu);
    }

    public void CreateManufacturer(IManufacturer manufacturer)
    {
        _dao.CreateNewManufacturer(manufacturer);
    }

    //DELETE
    public void DeleteManufacturer(int manufacturerId)
    {
        _dao.DeleteManufacturer(manufacturerId);
    }

    public void DeleteCPU(int cpuId)
    {
        _dao.DeleteCPU(cpuId);
    }

    //UPDATES
    public void UpdateManufacturer(IManufacturer manufacturer)
    {
        _dao.UpdateManufacturer(manufacturer);
    }

    public void UpdateCPU(ICPU cpu)
    {
        _dao.UpdateCPU(cpu);
    }

    //FILTER ON PRODUCT
    public IEnumerable<ICPU> FilterProductByProducer(string producerName)
    {
        return _dao.GetAllCPUs().Where(c => c.manufacturer.Name.Equals(producerName));
    }

    public IEnumerable<ICPU> FilterProductBySocketType(CPUSocketType socketType)
    {
        return _dao.GetAllCPUs().Where(c => c.SocketType.Equals(socketType));
    }

    public IEnumerable<ICPU> FilterProductByCores(int cores)
    {
        return _dao.GetAllCPUs().Where(c => c.Cores.Equals(cores));
    }

    //FILTER ON PRODUCER
    public IEnumerable<IManufacturer> FilterProducerByAddress(string address)
    {
        return _dao.GetAllManufaturers().Where(p => p.Address.Equals(address));
    }

    //SEARCHES
    public IEnumerable<ICPU> SearchCPUByName(string name)
    {
        return _dao.GetAllCPUs().Where(c => c.Name.ToLower().Contains(name.ToLower()));
    }

    public IEnumerable<IManufacturer> SearchProducerByName(string name)
    {
        return _dao.GetAllManufaturers().Where(p => p.Name.ToLower().Contains(name.ToLower()));
    }

    // Unique val's
    public IEnumerable<string> GetUniqueAddresses()
    {
        return _dao.GetAllManufaturers().Select(p => p.Address).Distinct().ToList();
    }

    public IEnumerable<string> GetUniqueCores()
    {
        return _dao.GetAllCPUs().Select(c => c.Cores.ToString()).Distinct().ToList();
    }
}
