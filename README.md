# CatJson

Ett exempel på **repository-mönstret** — katter sparade i JSON.

## Varför delar vi upp koden i klasser?

Under live-kodningen slutade lista-funktionen att fungera — listan var alltid tom. Felet visade sig vara en saknad `else`-gren i `LoadCats()`: utan den nollställdes listan varje gång filen inte hittades.

Men det viktiga är inte felet i sig — det är hur snabbt vi hittade det.

Eftersom `CatRepository` ansvarar för **all** CRUD-logik och filhantering kunde vi direkt avgränsa söket:

- `AddCat` fungerade — listan uppdaterades rätt
- `SaveCats` fungerade — filen skapades
- `LoadCats` — **här var felet**

Istället för att leta igenom hela programmet visste vi exakt vilken klass och vilken metod som var problemet. Det är repository-mönstrets värde i praktiken.

## Struktur

| Fil | Ansvar |
|-----|--------|
| `Program.cs` | Flöde — ladda, visa meny, spara |
| `CatRepositoryBase.cs` | Kontrakt — definierar vad ett repository ska kunna göra |
| `CatRepository.cs` | Implementation — CRUD + JSON-lagring |
| `Models/Cat.cs` | Datamodell |

## Repository-mönstret

```
Program.cs
    │
    └── CatRepository (ärver CatRepositoryBase)
            ├── AddCat()
            ├── GetCat()
            ├── UpdateCat()
            ├── DeleteCat()
            ├── LoadCats()   ← laddning från fil
            └── SaveCats()   ← sparning till fil
```

`Program.cs` vet inte hur katter sparas — det är `CatRepository`s sak. Vill vi byta JSON mot en databas senare räcker det att skriva en ny klass som ärver `CatRepositoryBase`. Ingenting annat behöver ändras.

## Nyckelbegrepp

- **Abstract class** — `CatRepositoryBase` tvingar fram ett gemensamt gränssnitt
- **Single Responsibility** — varje klass gör en sak, felsökning blir avgränsat
- **Repository-mönstret** — separerar dataåtkomst från övrig logik
