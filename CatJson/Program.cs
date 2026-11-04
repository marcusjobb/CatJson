// Programmets startpunkt — hanterar flödet, ingenting annat.
// All logik för katter och filer ligger i CatRepository.
Console.WriteLine("Hello, CatJson!");

var cr = new CatRepository();
cr.FilePath = @"c:\git";
cr.LoadCats(); // laddar befintliga katter från fil innan menyn visas

Console.WriteLine("1 - lista alla katter");
Console.WriteLine("2 - Lägg till en katt");

var input = Console.ReadLine();

switch (input)
{
    case "1":
        foreach (Cat cat in cr.cats)
        {
            Console.WriteLine(cat.Name);
        }
        break;
    case "2":
        var name = Console.ReadLine();
        cr.AddCat(new Cat { Name = name, Age = 1 });
        break;
}

cr.SaveCats(); // sparar alltid tillbaka till fil innan programmet avslutas