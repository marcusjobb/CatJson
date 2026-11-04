public abstract class CatRepositoryBase
{
    public abstract List<Cat> cats { get; set;  }
    public abstract void AddCat(Cat cat);
    public abstract void DeleteCat(Cat cat);
    public abstract Cat? GetCat(string name);
    public abstract void LoadCats();
}