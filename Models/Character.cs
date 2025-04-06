using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickAndMortyMauiApp.Models
{
    public class Character
    {
        public int Id { get; set; } // klucz główny
        public string Name { get; set; }
        public string Status { get; set; }
        public string Species { get; set; }
        public string Gender { get; set; }

        // Relacja: jeden bohater – wiele epizodów
        public List<CharacterEpisode> CharacterEpisodes { get; set; } = new();
    }
}
