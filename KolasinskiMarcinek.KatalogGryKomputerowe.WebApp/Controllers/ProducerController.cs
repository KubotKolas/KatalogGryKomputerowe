using KolasinskiMarcinek.KatalogGryKomputerowe.BL;
using KolasinskiMarcinek.KatalogGryKomputerowe.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.WebApp.Controllers;

public class ProducersController : Controller
{
    private readonly BusinessLogic _bl;

    public ProducersController(BusinessLogic bl)
    {
        _bl = bl;
    }

    public IActionResult Index(string searchString)
    {
        var producers = _bl.GetAllProducers();

        if (!string.IsNullOrEmpty(searchString))
        {
            producers = producers.Where(p =>
                p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                || p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)
            );
        }

        ViewData["CurrentFilter"] = searchString;
        return View(producers);
    }

    [HttpGet]
    public IActionResult Create() => View(new ProducerViewModel());

    [HttpPost]
    public IActionResult Create(ProducerViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Używamy klasy Producer z DAOMock lub stwórz uniwersalną w BL
        var newProducer = new BL.Producer { Name = model.Name, Address = model.Address };

        _bl.CreateNewProducer(newProducer);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var prod = _bl.GetAllProducers().FirstOrDefault(p => p.Id == id);
        if (prod == null)
            return NotFound();

        return View(
            new ProducerViewModel
            {
                Id = prod.Id,
                Name = prod.Name,
                Address = prod.Address,
            }
        );
    }

    [HttpPost]
    public IActionResult Edit(int id, ProducerViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var updated = new BL.Producer
        {
            Id = id,
            Name = model.Name,
            Address = model.Address,
        };
        _bl.UpdateProducer(updated);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var prod = _bl.GetAllProducers().FirstOrDefault(p => p.Id == id);
        if (prod == null)
            return NotFound();
        return View(prod);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        _bl.DeleteProducer(id);
        return RedirectToAction(nameof(Index));
    }
}
