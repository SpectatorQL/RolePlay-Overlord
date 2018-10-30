Gameplay
{
    Rozdzielone sterowanie GM/klienci
    {
        GM
        {
            Sterowanie w panelu zarządzania sesją
            Sterowanie myszką do ustawiania pionków
            Prosty kalkulator
        }
        Klient
        {
            Sterowanie w panelu postaci
        }
        Rzucanie kostką
    }
}

Sieć
{
    Architektura server + GM
    {
        GM wysyła "chęć" zmiany elementów pokoju
        GM wysyła "chęć" zmiany statystyk i klas graczy wraz z modelami
    }
    Serwowanie graczom ich aktualnych statystyk
    {
        Klienci nie mają możliwości kontroli sesji, poza oglądaniem swoich statystyk i rzucaniem kostkami
    }
    Customowe lobby dla potrzeb gry
}

Rozszerzenia edytora
{
    Post-build skrypt, przerzucający tymczasowe assety do folderu z buildem
    {
        Specjalne foldery na pliki tymczasowe
    }
    Skrypt, który włącza grę od sceny Menu jeżeli jesteśmy na scenie MainScene
}

Mody
{
    /*
        Im dłużej nad tym myślę, tym więcej przemawia za tym, żeby to też wrzucić do projektu.
        Zobaczymy jak to wyjdzie w praniu.
    */
    Ładowanie prostych zasobów (tekstury + audio)
    {
        Tworzenie thumbnaili dla tekstur przed załadowaniem do AssetBundles
    }
    // Animacji najprawdopodobniej nie będzie
    Projekt w Unity dla ładowania meshy (AssetBundles)
    {
        Odpowiednia hierarchia folderów
        TreeView do wyświetlania assetów do załadowania wraz z opcją przypisania ich do odpowiedniej paczki z modem
        Dokumentacja dla użytkowników
    }
    Customowy format dla plików tekstowych modów (xml? json?)
}

UI
{
    Lobby
    Panel GMa - zarządzanie modem
    Panel GMa - ustawianie statystyk graczy
    Czytelne formatowanie zawartości plików tekstowych systemu gry
}