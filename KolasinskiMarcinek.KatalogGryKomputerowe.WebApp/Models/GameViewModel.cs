using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.WebApp.Models;

public class GameViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nazwa gry jest wymagana")]
    [MinLength(3, ErrorMessage = "Nazwa gry musi mieć minimum 3 znaki")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Rok wydania jest wymagany")]
    [Range(1950, 2100, ErrorMessage = "Nieprawidłowy rok wydania")]
    public int ReleaseYear { get; set; }

    public bool Multiplayer { get; set; }

    [Required(ErrorMessage = "Wybierz gatunek")]
    public CORE.GameGenre Genre { get; set; }

    [Required(ErrorMessage = "Wybierz producenta")]
    public int SelectedProducerId { get; set; }

    // Lista do wypełnienia dropdowna (wymóg 6.2)
    public List<SelectListItem>? Producers { get; set; }
}
