using PhoneBook.Controller;
using PhoneBook.Models;
using PhoneBook.Services;
using PhoneBook.Validation;
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

        public async Task OnStart()
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
                        var contact = NewContactPrompt();
                       await _contactController.InsertContactAsync(contact);
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

        private Contact NewContactPrompt()
        {
            var contact = new Contact
            {
                FirstName = PromptValidated("Enter [green]first name[/]:", InputValidator.IsValidName),
                LastName = PromptValidated("Enter [green]last name[/]:", InputValidator.IsValidName),
                OrganizationName = AnsiConsole.Ask("Enter [green]organization[/] (optional):", string.Empty),
                JobTitle = AnsiConsole.Ask("Enter [green]job title[/] (optional):", string.Empty),
                Notes = AnsiConsole.Ask("Enter [green]notes[/] (optional):", string.Empty)
            };

            contact.PhoneNumbers.Add(new PhoneNumberDetail
            {
                Number = PromptValidated("Enter [green]phone number[/]:", InputValidator.IsValidPhoneNumber),
                Label = AnsiConsole.Prompt(new SelectionPrompt<LabelType>().Title("Select [green]label type[/]:").AddChoices(LabelType.Mobile, LabelType.Home, LabelType.Work, LabelType.Other))
            });
            while (AnsiConsole.Confirm("Add another phone number?", false))
            {
                contact.PhoneNumbers.Add(new PhoneNumberDetail
                {
                    Number = PromptValidated("Enter [green]phone number[/]:", InputValidator.IsValidPhoneNumber),
                    Label = AnsiConsole.Prompt(new SelectionPrompt<LabelType>().Title("Select [green]label type[/]:").AddChoices(LabelType.Mobile, LabelType.Home, LabelType.Work, LabelType.Other))
                });
            }

            while (AnsiConsole.Confirm("Add an email address?", false))
            {
                contact.Emails.Add(new EmailDetail
                {
                    EmailAddress = PromptValidated("Enter [green]email[/]:", InputValidator.IsValidEmail),
                    Label = AnsiConsole.Prompt(new SelectionPrompt<LabelType>().Title("Select [green]label type[/]:").AddChoices(LabelType.Mobile, LabelType.Home, LabelType.Work, LabelType.Other))
                });
            }

            return contact;
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
