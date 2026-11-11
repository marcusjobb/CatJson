# CatJson — UML-översikt

Översikt över hela projektet i Mermaid. Diagrammen är genererade utifrån
källkoden i `CatJson`-projektet (`.NET 10`, konsolapp).

---

## Klassdiagram

```mermaid
classDiagram
    direction TB

    class Cat {
        +string ID
        +string Name
        +int Age
    }

    class Meow {
        +bool IsEvil
        +SayMeow() void
    }

    class CatRepositoryBase {
        <<abstract>>
        +List~Cat~ cats
        +AddCat(Cat cat)* void
        +DeleteCat(Cat cat)* void
        +GetCat(string name)* Cat?
        +LoadCats()* void
    }

    class CatRepository {
        +List~Cat~ cats
        +string FilePath
        +AddCat(Cat cat) void
        +GetCat(string name) Cat?
        +UpdateCat(Cat cat) void
        +DeleteCat(Cat cat) void
        +SaveCats() void
        +LoadCats() void
    }

    class CatSqliteRepository {
        -string _connectionString
        +CatSqliteRepository(string dbPath)
        -CreateTableIfNotExists() void
        +LoadCats() void
        +AddCat(Cat cat) void
        +GetCat(string name) Cat?
        +DeleteCat(Cat cat) void
    }

    class Program {
        <<entry point>>
        +Main() void
    }

    Cat <|-- Meow : ärver
    CatRepositoryBase <|-- CatRepository : ärver
    CatRepositoryBase <|-- CatSqliteRepository : ärver

    CatRepositoryBase o-- "0..*" Cat : innehåller

    Program ..> CatRepository : använder
    Program ..> Cat : skapar
```

**Läsning:**
- `Cat` är datamodellen — en katt med `ID`, `Name`, `Age`.
- `Meow` ärver `Cat` (oavslutad/platshållare — `SayMeow()` kastar `NotImplementedException`).
- `CatRepositoryBase` är ett abstrakt kontrakt för CRUD på katter.
- `CatRepository` lagrar till JSON-fil (`Catlist.json`).
- `CatSqliteRepository` lagrar till SQLite-databas — samma kontrakt, annan lagring.
- `Program` är startpunkten och väljer vilket repository som används.

---

## Beroenden & paket

```mermaid
flowchart LR
    Program --> CatRepository
    Program -.->|"valfritt (utkommenterat)"| CatSqliteRepository
    CatRepository --> Cat
    CatSqliteRepository --> Cat
    CatRepository -->|System.Text.Json| Json["Catlist.json"]
    CatSqliteRepository -->|Microsoft.Data.Sqlite| DB[("SQLite<br/>tabell: Cats")]
    Meow --> Cat
```

---

## Programflöde (menyn)

```mermaid
flowchart TD
    Start([Start]) --> Hello["Skriv ut: Hello, CatJson!"]
    Hello --> NewRepo["new CatRepository()<br/>FilePath = c:\\git"]
    NewRepo --> Load["cr.LoadCats()<br/>läs Catlist.json"]
    Load --> Menu["Visa meny:<br/>1 - lista alla katter<br/>2 - lägg till katt"]
    Menu --> Choice{"Val?"}
    Choice -->|"1"| List["Skriv ut alla cat.Name"]
    Choice -->|"2"| Add["Läs namn<br/>AddCat(new Cat Name, Age=1)"]
    List --> Save["cr.SaveCats()<br/>skriv till fil"]
    Add --> Save
    Save --> End([Slut])
```

---

## Sekvensdiagram — lägg till en katt

```mermaid
sequenceDiagram
    participant U as Användare
    participant P as Program
    participant R as CatRepository
    participant F as Catlist.json

    U->>P: Startar programmet
    P->>R: new CatRepository()
    P->>R: LoadCats()
    R->>F: File.Exists / ReadAllText
    F-->>R: JSON-lista
    R-->>P: cats fylld
    P->>U: Visar meny
    U->>P: "2"
    U->>P: Namn på katt
    P->>R: AddCat(new Cat)
    R-->>P: cats.Add(cat)
    P->>R: SaveCats()
    R->>F: WriteAllText(JSON)
```

---

## Lagring — två implementationer av samma kontrakt

```mermaid
flowchart TB
    Base["CatRepositoryBase<br/>(abstrakt kontrakt)"]
    Base --> Json["CatRepository<br/>JSON-fil"]
    Base --> Sql["CatSqliteRepository<br/>SQLite"]

    Json --> JsonOps["AddCat → cats.Add<br/>GetCat → FirstOrDefault<br/>UpdateCat → sätt Age<br/>DeleteCat → cats.Remove<br/>SaveCats → WriteAllText<br/>LoadCats → ReadAllText"]

    Sql --> SqlOps["AddCat → INSERT + GUID<br/>GetCat → FirstOrDefault (minne)<br/>DeleteCat → DELETE by ID<br/>LoadCats → SELECT *<br/>CreateTableIfNotExists"]
```

> **Not:** `CatRepositoryBase` saknar `SaveCats()` och `UpdateCat()` i kontraktet —
> de finns bara på `CatRepository`. `CatSqliteRepository` sparar istället direkt
> vid varje `AddCat`/`DeleteCat` och behöver därför ingen separat `SaveCats()`.
