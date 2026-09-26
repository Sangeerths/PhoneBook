using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Repository;
using PhoneBook.UI;

Env.Load();
using (var context = new PhoneBookContext())
{
    context.Database.Migrate();

    if (!context.Contacts.Any() )
    {
        context.SeedDataFromExcel();
    }
}
ConsoleMenu menu = new();
await menu.OnStart();
