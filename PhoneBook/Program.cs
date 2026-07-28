using DotNetEnv;
using PhoneBook.UI;

Env.Load();
ConsoleMenu menu = new ConsoleMenu();
await menu.OnStart();
