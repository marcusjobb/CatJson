using System.Text.Json;

// Konkret implementation av repository-mönstret med JSON-lagring.
// All CRUD-logik och filhantering finns här — inget annat ställe.
public class CatRepository : CatRepositoryBase
{
    public override List<Cat> cats { get; set; } = new(); // minneslistan — vår "databas" under körning
    public string FilePath { get; set; }

    // Create — lägg till en katt i listan
    public override void AddCat(Cat cat)
    {
        cats.Add(cat);
    }

    // Read — hämta en specifik katt via namn, null om den inte finns
    public override Cat? GetCat(string name)
    {
        return cats.FirstOrDefault(c => c.Name == name);
    }

    // Update — vi hämtar en referens till rätt objekt och ändrar direkt på den
    public void UpdateCat(Cat cat)
    {
        var oldCat = GetCat(cat.Name);
        oldCat.Age = cat.Age;
    }

    // Delete — Remove på exakt objektet garanterar att rätt katt tas bort
    public override void DeleteCat(Cat cat)
    {
        cats.Remove(cat);
    }

    // Serialiserar listan till JSON och skriver till fil
    public void SaveCats()
    {
        string filename = Path.Combine(FilePath, "Catlist.json");
        string json = JsonSerializer.Serialize(cats);
        File.WriteAllText(filename, json);
    }

    // Laddar katter från fil — else nollställer INTE listan om filen saknas,
    // den skapar en tom lista istället (klassiskt buggtillfälle utan else-grenen)
    public override void LoadCats()
    {
        string filename = Path.Combine(FilePath, "Catlist.json");
        if (File.Exists(filename))
        {
            string json = File.ReadAllText(filename);
            cats = JsonSerializer.Deserialize<List<Cat>>(json);
        }
        else
            cats = new();
    }
}