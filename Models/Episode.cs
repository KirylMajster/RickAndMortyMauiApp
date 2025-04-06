using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RickAndMortyMauiApp.Models
{
    public class Episode
    {
        public int Id { get; set; } // klucz główny
        public string Name { get; set; }
        public string EpisodeCode { get; set; }

        public List<CharacterEpisode> CharacterEpisodes { get; set; } = new();
    }
}
