using Microsoft.Data.Sqlite;

// SQLite-implementation av repository-kontraktet.
// Byter vi från JSON till SQLite räcker det att skapa den här klassen —
// Program.cs och basklassen behöver inte röras.
public class CatSqliteRepository : CatRepositoryBase
{
    public override List<Cat> cats { get; set; } = new();

    private readonly string _connectionString;

    public CatSqliteRepository(string dbPath)
    {
        _connectionString = $"Data Source={dbPath}";
        CreateTableIfNotExists();
    }

    // Skapar tabellen om den inte redan finns — körs en gång vid start
    private void CreateTableIfNotExists()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = """
            CREATE TABLE IF NOT EXISTS Cats (
                ID   TEXT PRIMARY KEY,
                Name TEXT NOT NULL,
                Age  INTEGER NOT NULL
            )
            """;
        cmd.ExecuteNonQuery();
    }

    // Laddar alla katter från databasen in i minneslistan
    // Inte optimal lösning men OK för denna enkla applikation
    // I normala fall skulle vi låtit databasen hantera allt
    public override void LoadCats()
    {
        cats = new();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT ID, Name, Age FROM Cats";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            cats.Add(new Cat
            {
                ID   = reader.GetString(0),
                Name = reader.GetString(1),
                Age  = reader.GetInt32(2)
            });
        }
    }

    // Create — sparar direkt till databasen
    public override void AddCat(Cat cat)
    {
        cat.ID = Guid.NewGuid().ToString();
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "INSERT INTO Cats (ID, Name, Age) VALUES ($id, $name, $age)";
        cmd.Parameters.AddWithValue("$id",   cat.ID);
        cmd.Parameters.AddWithValue("$name", cat.Name);
        cmd.Parameters.AddWithValue("$age",  cat.Age);
        cmd.ExecuteNonQuery();
        cats.Add(cat);
    }

    // Read — söker i minneslistan (redan laddad via LoadCats)
    public override Cat? GetCat(string name)
    {
        return cats.FirstOrDefault(c => c.Name == name);
    }

    // Delete — tar bort från databasen och minneslistan
    public override void DeleteCat(Cat cat)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM Cats WHERE ID = $id";
        cmd.Parameters.AddWithValue("$id", cat.ID);
        cmd.ExecuteNonQuery();
        cats.Remove(cat);
    }
}
