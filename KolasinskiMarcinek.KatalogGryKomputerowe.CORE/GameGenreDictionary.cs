using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KolasinskiMarcinek.KatalogGryKomputerowe.CORE
{
    public static class GameGenreTranslator
    {
        private static readonly Dictionary<GameGenre, string> Translations = new()
        {
            { GameGenre.Adventure, "Przygodowa" },
            { GameGenre.RPG, "RPG" },
            { GameGenre.Action, "Akcja" },
            { GameGenre.TowerDefense, "Tower Defense" },
            { GameGenre.RTS, "RTS" },
            { GameGenre.TurnBaseStrategy, "Strategia Turowa" },
            { GameGenre.Stealth, "Skradanka" },
            { GameGenre.Survival, "Przetrwanie" },
            { GameGenre.MOBA, "MOBA" },
            { GameGenre.Racing, "Wyścigówka" },
            { GameGenre.Other, "Inne" }
        };

        public static GameGenre GetGenreByTranslation(string translatedName)
        {
            foreach (var pair in Translations)
            {
                if (pair.Value.Equals(translatedName, StringComparison.OrdinalIgnoreCase))
                    return pair.Key;
            }
            return GameGenre.Other;
        }

        public static string Translate(GameGenre genre) =>
            Translations.TryGetValue(genre, out var translation) ? translation : genre.ToString();

        public static List<string> GetTranslatedValues() =>
            Translations.Values.ToList();
    }
}
