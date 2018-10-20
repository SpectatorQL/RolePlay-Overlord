Gameplay
{
    Poruszanie kamerą
    Rozdzielone sterowanie GM/klienci
    {
        GM
        {
            Sterowanie kamerą (latanie)
            Sterowanie w panelu zarządzania sesją
            Sterowanie myszką do ustawiania pionków
        }
        Klient
        {
            Sterowanie kamerą (ograniczone)
            Sterowanie w panelu postaci
        }
        Rzucanie kostką
    }
}

Sieć
{
    Podstawowe aspekty sieciowe
    {
        Customowe lobby dla potrzeb gry
    }
    Architektura server + GM
    {
        GM wysyła "chęć" zmiany elementów pokoju
        GM wysyła "chęć" zmiany statystyk i klas graczy wraz z modelami
    }
    Serwowanie graczom ich aktualnych statystyk
    {
        Klienci nie mają możliwości kontroli sesji, poza oglądaniem swoich statystyk i rzucaniem kostkami
    }
}

Mody
{
    /*
        Im dłużej nad tym myślę, tym więcej przemawia za tym, żeby to też wrzucić do projektu.
        Zobaczymy jak to wyjdzie w praniu.
    */
    Ładowanie prostych zasobów (tekstury + audio)
    Projekt w Unity dla ładowania meshy wraz z animacjami (AssetBundles)
    Dokumentacja do ww.
    Customowy format dla plików tekstowych systemu modów (xml? json?)
}

UI
{
    Lobby
    Panel GMa - zarządzanie modem
    Panel GMa - ustawianie statystyk graczy
    Czytelne formatowanie zawartości plików tekstowych systemu gry
}