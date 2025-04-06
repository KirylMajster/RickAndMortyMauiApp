using RickAndMortyMauiApp.Models;
using RickAndMortyMauiApp.Services;
using RickAndMortyMauiApp.Data;

namespace RickAndMortyMauiApp;

public partial class MainPage : ContentPage
{
    private CharacterService _service = new();

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnDownloadClicked(object sender, EventArgs e)
    {
        if (int.TryParse(entryCount.Text, out int count))
        {
            var characters = await _service.GetCharactersAsync(count);
            await _service.SaveToDatabaseAsync(characters);

            await DisplayAlert("Sukces", $"Pobrano i zapisano {characters.Count} postaci.", "OK");
        }
        else
        {
            await DisplayAlert("Błąd", "Wprowadź poprawną liczbę postaci.", "OK");
        }
    }

    private void OnLoadFromDbClicked(object sender, EventArgs e)
    {
        using var db = new AppDbContext();
        var characters = db.Characters.ToList();

        characterListView.ItemsSource = characters;
    }

    private void OnCharacterTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Character selectedCharacter)
        {
            using var db = new AppDbContext();

            // Załaduj epizody dla tej postaci
            var episodes = db.CharacterEpisodes
                .Where(ce => ce.CharacterId == selectedCharacter.Id)
                .Select(ce => ce.Episode)
                .ToList();

            string episodeList = string.Join("\n", episodes.Select(ep => $"{ep.EpisodeCode}: {ep.Name}"));

            if (episodes.Count == 0)
                episodeList = "Brak epizodów";

            DisplayAlert($"Epizody {selectedCharacter.Name}", episodeList, "OK");
        }
    }

    private void OnFilterClicked(object sender, EventArgs e)
    {
        string filter = filterEntry.Text?.Trim().ToLower();

        using var db = new AppDbContext();

        var filteredCharacters = db.CharacterEpisodes
            .Where(ce =>
                ce.Episode.EpisodeCode.ToLower().Contains(filter) ||
                ce.Episode.Name.ToLower().Contains(filter) ||
                ce.Character.Status.ToLower().Contains(filter) ||
                ce.Character.Species.ToLower().Contains(filter)
            )
            .Select(ce => ce.Character)
            .Distinct()
            .ToList();

        characterListView.ItemsSource = filteredCharacters;
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (characterListView.SelectedItem is Character selectedCharacter)
        {
            bool confirm = await DisplayAlert("Potwierdzenie", $"Usunąć {selectedCharacter.Name} z bazy?", "Tak", "Nie");

            if (confirm)
            {
                using var db = new AppDbContext();

                // Usuń relacje z epizodami
                var relations = db.CharacterEpisodes.Where(ce => ce.CharacterId == selectedCharacter.Id);
                db.CharacterEpisodes.RemoveRange(relations);

                // Usuń postać
                var character = db.Characters.FirstOrDefault(c => c.Id == selectedCharacter.Id);
                if (character != null)
                {
                    db.Characters.Remove(character);
                    db.SaveChanges();

                    await DisplayAlert("Sukces", $"{character.Name} został usunięty!", "OK");

                    // Odśwież widok
                    characterListView.ItemsSource = db.Characters.ToList();
                }
            }
        }
        else
        {
            await DisplayAlert("Uwaga", "Nie zaznaczono postaci!", "OK");
        }
    }


}
