using PhoneBook.Controller;
using PhoneBook.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace PhoneBook.UI
{
    public class ConsoleMenu
    {
        private readonly ContactController _contactController;
        private enum MenuOptions
        {
            InsertContact,
            DeleteContact,
            ModifyContact,
            ViewContact,
            ViewAllContacts,
            Exit
        }

        public ConsoleMenu()
        {
            _contactController = new ContactController();
        }

        public void OnStart()
        {
            bool isRunning = true;
            while  (isRunning)
            {
                var choice = AnsiConsole.Prompt(new SelectionPrompt<MenuOptions>().Title("Choose your operation").AddChoices(
                    MenuOptions.InsertContact,
                    MenuOptions.DeleteContact,
                    MenuOptions.ModifyContact,
                    MenuOptions.ViewContact,
                    MenuOptions.ViewAllContacts,
                    MenuOptions.Exit));

                switch (choice)
                {
                    case MenuOptions.InsertContact:
                        _contactController.InsertContactAsync();
                        break;
                    case MenuOptions.DeleteContact:
                        _contactController.DeleteContactAsync();
                        break;
                    case MenuOptions.ModifyContact:
                        _contactController.ModifyContact();
                        break;
                    case MenuOptions.ViewContact:
                        _contactController.ViewContact();
                        break;
                    case MenuOptions.ViewAllContacts:
                        _contactController.ViewAllContacts();
                        break;
                    case MenuOptions.Exit:
                        isRunning = false;
                        break;
                }
            }
        }

        private string PromptValidated(string promptText, Func<string, bool> isValid)
        {
            string input;
            do
            {
                input = AnsiConsole.Ask<string>(promptText);
                if (!isValid(input))
                    AnsiConsole.MarkupLine("[red]Invalid input, please try again.[/]");
            } while (!isValid(input));

            return input;
        }
    }
}
