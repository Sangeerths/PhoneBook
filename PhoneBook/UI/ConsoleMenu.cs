using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace PhoneBook.UI
{
    public class ConsoleMenu
    {
        private enum MenuOptions
        {
            InsertContact,
            DeleteContact,
            ModifyContact,
            ViewContact,
            ViewAllContacts,
            Exit
        }

        public ConsoleMenu() { }

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
                        break;
                    case MenuOptions.DeleteContact:
                        break;
                    case MenuOptions.ModifyContact:
                        break;
                    case MenuOptions.ViewContact:
                        break;
                    case MenuOptions.ViewAllContacts:
                        break;
                    case MenuOptions.Exit:
                        isRunning = false;
                        break;
                }
            }
        }

        public void InsertContact()
        {

        }

        public void DeleteContact()
        {

        }

        public void ModifyContact()
        {

        }

        public void ViewContact()
        {

        }

        public void ViewAllContacts()
        {

        }
    }
}
