# Projekt-ILock-Aplikacja-testowa
Aplikacja testowa utworzona w celach rekrutacji

Instrukcja obsługi aplikacji „Aplikacja testowa ILock”
Przygotowane dane wejściowe w postaci zdarzeń w formacie JSON przedstawiają 10 wydarzeń, które będą dodane do kalendarza Google.
```ruby
jsonString = "{\"EventsList\":[{\"DateStart\":\"2019-05-14 12:00:00\",\"DateEnd\":\"2019-05-14 12:30:00\",\"Summary\":\"Wydarzenie 1\",\"Description\":\"Opis wydarzenia 1\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-14 14:13:00\",\"DateEnd\":\"2019-05-14 14:32:00\",\"Summary\":\"Wydarzenie 2\",\"Description\":\"Opis wydarzenia 2\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-14 08:20:00\",\"DateEnd\":\"2019-05-14 08:35:00\",\"Summary\":\"Wydarzenie 3\",\"Description\":\"Opis wydarzenia 3\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-14 15:00:00\",\"DateEnd\":\"2019-05-14 15:30:00\",\"Summary\":\"Wydarzenie 4\",\"Description\":\"Opis wydarzenia 4\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-15 09:00:00\",\"DateEnd\":\"2019-05-15 09:30:00\",\"Summary\":\"Wydarzenie 5\",\"Description\":\"Opis wydarzenia 5\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-15 10:00:00\",\"DateEnd\":\"2019-05-15 11:30:00\",\"Summary\":\"Wydarzenie 6\",\"Description\":\"Opis wydarzenia 6\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-15 12:00:00\",\"DateEnd\":\"2019-05-15 12:10:00\",\"Summary\":\"Wydarzenie 7\",\"Description\":\"Opis wydarzenia 7\",\"Location\":\"Gniezno\"}," +
            "{\"DateStart\":\"2019-05-15 13:40:00\",\"DateEnd\":\"2019-05-15 13:59:00\",\"Summary\":\"Wydarzenie 8\",\"Description\":\"Opis wydarzenia 8\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-16 12:00:00\",\"DateEnd\":\"2019-05-16 12:30:00\",\"Summary\":\"Wydarzenie 9\",\"Description\":\"Opis wydarzenia 9\",\"Location\":\"Poznań\"}," +
            "{\"DateStart\":\"2019-05-14 14:00:00\",\"DateEnd\":\"2019-05-14 14:05:00\",\"Summary\":\"Wydarzenie 10\",\"Description\":\"Opis wydarzenia 10\",\"Location\":\"Poznań\"}]}"
```
Przy pierwszym uruchomieniu aplikacja sprawdza, czy kalendarz do którego dodawane są wydarzenia jest pusty, jeśli tak, dane w formacie JSON są przetwarzane i dodawane do kalendarza.
 
 ![alt text](https://i.ibb.co/8ztVgGM/1111.png)

Poniżej został przedstawiony zrzut ekranu pokazujący uruchomioną aplikację. Widoczna jest lista z informacjami na temat wydarzeń, posortowanymi zgodnie z datą rozpoczęcia.

 ![alt text](https://i.ibb.co/5K2172V/Bez-tytu-u.jpg)

Dodatkowo do dyspozycji są trzy przyciski 
- „Usuń wszystkie wydarzenia” - czyści wszystkie wydarzenia w kalendarzu, 
- „Odśwież wydarzenia” - pobiera aktualne wydarzenia z kalendarza,
- „Dodaj wydarzenia z JSON” – jeśli kalendarz jest pusty dodaje wydarzenia z dany wejściowych w postaci JSON. 
