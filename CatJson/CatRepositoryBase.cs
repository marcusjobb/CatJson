// Abstrakt basklass — definierar kontraktet för ett katt-repository.
// Vill vi byta lagringsmetod (t.ex. databas istället för JSON) ärver vi den här klassen
// och implementerar metoderna på nytt utan att ändra något annat i programmet.
public abstract class CatRepositoryBase
{
    public abstract List<Cat> cats { get; set; }
    public abstract void AddCat(Cat cat);
    public abstract void DeleteCat(Cat cat);
    public abstract Cat? GetCat(string name);
    public abstract void LoadCats();
}