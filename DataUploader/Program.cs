using DataUploader;
using DataUploader.Data;
using System.Configuration;

var defaultImagePath = ConfigurationManager.AppSettings.Get("DefaultImagePath");
List<Product> Products = new();
List<Category> Categories = new();
#region ProductsForm
Products.Add(new Product("Мыло", 10, 1, defaultImagePath));
Products.Add(new Product("Стиральный порошок", 12, 1, defaultImagePath));

Products.Add(new Product("Огурцы 1кг", 30, 2, defaultImagePath));
Products.Add(new Product("Помидоры 1кг", 28, 2, defaultImagePath));
Products.Add(new Product("Яблоки", 50, 2, defaultImagePath));
Products.Add(new Product("Груши", 60, 2, defaultImagePath));

Products.Add(new Product("Булочка с маком", 5, 3, defaultImagePath));
Products.Add(new Product("Булочка с изюмом", 5, 3, defaultImagePath));
Products.Add(new Product("Булочка с вишней", 5, 3, defaultImagePath));
Products.Add(new Product("Батон нарезной", 8, 3, defaultImagePath));
Products.Add(new Product("Багет", 15, 3, defaultImagePath));

Products.Add(new Product( "Вертолёт", 150, 4, defaultImagePath));
Products.Add(new Product( "Пистолет", 100, 4, defaultImagePath));
Products.Add(new Product( "Машина", 80, 4, defaultImagePath));

Products.Add(new Product( "Пломбир", 14, 5, defaultImagePath));
Products.Add(new Product( "Эскимо", 20, 5, defaultImagePath));
#endregion

#region CategoriesForm
Categories.Add(new Category("Бытовая химия",  defaultImagePath));
Categories.Add(new Category("Овощи и фрукты",  defaultImagePath));
Categories.Add(new Category("Хлеб",  defaultImagePath));
Categories.Add(new Category("Игрушки",  defaultImagePath));
Categories.Add(new Category("Мороженное",  defaultImagePath));
#endregion

using (ApplicationContext db = new ApplicationContext())
{ 
    foreach (Product product in Products)
    {
        db.Add(product);
    }
    db.SaveChanges();
    foreach (Category category in Categories)
    {
        db.Add(category);
    }
    db.SaveChanges();
};

Console.WriteLine("Данные успешно добавлены!");
Console.ReadLine();




