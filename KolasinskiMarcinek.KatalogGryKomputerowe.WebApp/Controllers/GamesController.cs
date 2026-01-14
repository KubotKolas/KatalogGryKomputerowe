using System.Diagnostics;
using KolasinskiMarcinek.KatalogGryKomputerowe.BL;
using KolasinskiMarcinek.KatalogGryKomputerowe.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.WebApp.Controllers;

public class GamesController : Controller
{
    private readonly BusinessLogic _bl;

    public GamesController(BusinessLogic bl)
    {
        _bl = bl;
    }

    public IActionResult Index()
    {
        var games = _bl.GetAllGames();
        return View(games);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new GameViewModel
        {
            // Pobieramy producentów i zamieniamy na listę do Selecta (wymóg 6.2)
            Producers = _bl.GetAllProducers()
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
                .ToList(),
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Create(GameViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Jeśli błąd walidacji, przeładuj listę producentów i wróć do formularza (wymóg 6.3)
            model.Producers = _bl.GetAllProducers()
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
                .ToList();
            return View(model);
        }

        // Tworzymy obiekt gry na podstawie danych z modelu
        // Pamiętaj, że musisz mieć konkretną klasę implementującą IGame (np. w BL lub WebApp)
        var newGame = new BL.Game // Tutaj użyj klasy z Twojego DAOMock lub stwórz lokalną w BL
        {
            Name = model.Name,
            ReleaseYear = model.ReleaseYear,
            Multiplayer = model.Multiplayer,
            Genre = model.Genre,
            Producer = _bl.GetAllProducers().First(p => p.Id == model.SelectedProducerId),
        };

        _bl.CreateNewGame(newGame);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var game = _bl.GetAllGames().FirstOrDefault(g => g.Id == id);
        if (game == null)
            return NotFound();

        var model = new GameViewModel
        {
            Id = game.Id,
            Name = game.Name,
            ReleaseYear = game.ReleaseYear,
            Multiplayer = game.Multiplayer,
            Genre = game.Genre,
            SelectedProducerId = game.Producer.Id,
            Producers = _bl.GetAllProducers()
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name,
                    Selected = p.Id == game.Producer.Id,
                })
                .ToList(),
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(int id, GameViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Producers = _bl.GetAllProducers()
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
                .ToList();
            return View(model);
        }

        var gameToUpdate = new BL.Game
        {
            Id = id,
            Name = model.Name,
            ReleaseYear = model.ReleaseYear,
            Multiplayer = model.Multiplayer,
            Genre = model.Genre,
            Producer = _bl.GetAllProducers().First(p => p.Id == model.SelectedProducerId),
        };

        _bl.UpdateGame(gameToUpdate);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var game = _bl.GetAllGames().FirstOrDefault(g => g.Id == id);
        if (game == null)
            return NotFound();
        return View(game);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        _bl.DeleteGame(id);
        return RedirectToAction("Index");
    }
}
