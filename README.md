# RickAndMortyMauiApp
Sprawozdanie – RickAndMortyMauiApp (.NET MAUI + SQLite)

RickAndMortyMauiApp to aplikacja stworzona w technologii .NET MAUI umożliwiająca:
- pobieranie postaci z publicznego API Rick and Morty,
- zapisywanie danych lokalnie w bazie SQLite,
- przeglądanie, filtrowanie oraz usuwanie postaci.

Aplikacja wykorzystuje lokalną bazę danych zbudowaną z pomocą Entity Framework Core.

Struktura bazy danych

W aplikacji zaimplementowano trzy modele danych:
- Character – postać (Id, Name, Status, Species, Gender),
- Episode – odcinek (Id, Name, EpisodeCode),
- CharacterEpisode – relacja wiele-do-wielu między postaciami a odcinkami.

Baza danych jest zarządzana przez AppDbContext z EF Core i tworzona automatycznie na podstawie migracji.
Funkcjonalności aplikacji
Pobieranie danych z API
Aplikacja umożliwia pobranie wybranej liczby postaci z API Rick and Morty. Metoda GetCharactersAsync pobiera dane JSON, parsuje je i tworzy obiekty Character i Episode.

Zapis do bazy danych
Zapis odbywa się z unikaniem duplikatów – aplikacja sprawdza, czy postać i epizody istnieją przed dodaniem.
Wyświetlanie i przeglądanie postaci
Postacie są ładowane z bazy danych i wyświetlane w liście. Po kliknięciu w postać użytkownik widzi, w jakich odcinkach wystąpiła.
Filtrowanie postaci
Aplikacja pozwala filtrować postacie na podstawie czterech pól:
- kod odcinka,
- nazwa odcinka,
- status postaci,
- gatunek postaci.
Filtrowanie odbywa się przy użyciu LINQ i jest niewrażliwe na wielkość liter.
Usuwanie postaci
Użytkownik może usunąć wybraną postać z bazy danych – aplikacja usuwa również relacje z epizodami.

Zarządzanie bazą danych
Aplikacja korzysta z Entity Framework Core 8.0.3 i lokalnej bazy danych SQLite.
Ścieżka do bazy jest dynamiczna (FileSystem.AppDataDirectory), co zapewnia kompatybilność z Androidem, iOS i Windows.

