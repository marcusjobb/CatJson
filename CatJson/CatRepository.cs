using System.Text.Json;

public class CatRepository : CatRepositoryBase
{
    public override List<Cat> cats { get;  set; } = new(); // <--- Fejkdatabas
    public string FilePath { get; set; } 

    // Create
    public override void AddCat(Cat cat)
    {
        cats.Add(cat);
    }
    
    // Read
    public override Cat? GetCat(string name)
    {
        return cats.FirstOrDefault(c => c.Name == name) as Cat;
    }
    
    // Update
    public void UpdateCat(Cat cat)
    {
        var oldCat = GetCat(cat.Name); // får en referens till katten
        oldCat.Age = cat.Age;
    }
    
    // Delete
    public override void DeleteCat(Cat cat)
    {
        cats.Remove(cat); // Säkraste sättet att veta att rätt katt raderas
    }

    public void SaveCats()
    {
        string filename = Path.Combine(FilePath, "Catlist.json");
        string json = JsonSerializer.Serialize(cats);
        File.WriteAllText(filename, json);
    }

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