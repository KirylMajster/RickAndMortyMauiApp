using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RickAndMortyMauiApp.Data;
using RickAndMortyMauiApp.Models;


namespace RickAndMortyMauiApp.Services;

public class CharacterService
{
    private readonly HttpClient _httpClient = new();

    public async Task<List<Character>> GetCharactersAsync(int count = 5)
    {
        var characters = new List<Character>();
        var episodeCache = new Dictionary<int, Episode>();

        using var db = new AppDbContext();

        for (int i = 1; i <= count; i++)
        {
            // Sprawdź, czy postać o tym ID już istnieje
            if (db.Characters.Any(c => c.Id == i))
                continue;

            // dopiero teraz pobieraj z API
            string url = $"https://rickandmortyapi.com/api/character/{i}";
            var json = await _httpClient.GetStringAsync(url);
            var obj = JObject.Parse(json);

            var character = new Character
            {
                Id = (int)obj["id"],
                Name = (string)obj["name"],
                Status = (string)obj["status"],
                Species = (string)obj["species"],
                Gender = (string)obj["gender"]
            };

            var episodeUrls = obj["episode"]!.Select(e => e.ToString()).ToList();

            foreach (var epUrl in episodeUrls)
            {
                var epJson = await _httpClient.GetStringAsync(epUrl);
                var epObj = JObject.Parse(epJson);

                var episodeId = (int)epObj["id"];

                if (!episodeCache.ContainsKey(episodeId))
                {
                    episodeCache[episodeId] = new Episode
                    {
                        Id = episodeId,
                        Name = (string)epObj["name"],
                        EpisodeCode = (string)epObj["episode"]
                    };
                }

                character.CharacterEpisodes.Add(new CharacterEpisode
                {
                    Character = character,
                    Episode = episodeCache[episodeId]
                });
            }

            characters.Add(character);
        }

        return characters;
    }


    public async Task SaveToDatabaseAsync(List<Character> characters)
    {
        using var db = new AppDbContext();

        foreach (var character in characters)
        {
            // Sprawdź, czy postać już istnieje
            if (!db.Characters.Any(c => c.Id == character.Id))
            {
                foreach (var ce in character.CharacterEpisodes)
                {
                    var ep = ce.Episode;

                    // Czy epizod już istnieje?
                    var existingEp = db.Episodes.FirstOrDefault(e => e.Id == ep.Id);

                    if (existingEp != null)
                    {
                        // Podmień referencję na istniejący epizod
                        ce.Episode = existingEp;
                    }
                    else
                    {
                        // Dodaj nowy epizod
                        db.Episodes.Add(ep);
                    }
                }

                db.Characters.Add(character);
            }
        }

        await db.SaveChangesAsync();
    }
}