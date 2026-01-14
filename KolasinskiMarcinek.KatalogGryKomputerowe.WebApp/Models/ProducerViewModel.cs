using System.ComponentModel.DataAnnotations;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.WebApp.Models;

public class ProducerViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nazwa producenta jest wymagana")]
    [MinLength(2, ErrorMessage = "Nazwa musi mieć co najmniej 2 znaki")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Adres jest wymagany")]
    public string Address { get; set; } = string.Empty;
}
