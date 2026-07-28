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
        private readonly ConsoleUI _consoleUI;
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
            _consoleUI = new ConsoleUI();
        }

        public async Task OnStart()
        {
            bool isRunning = true;
            while  (isRunning)
            {
                _consoleUI.ShowHeader();
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
                        _consoleUI.Pause();
                        break;
                    case MenuOptions.DeleteContact:
                        await DeleteContactFlowAsync();
                        _consoleUI.Pause();
                        break;
                    case MenuOptions.ModifyContact:
                        await ModifyContactFlowAsync();
                        _consoleUI.Pause();
                        break;
                    case MenuOptions.ViewContact:
                        await ViewContactFlowAsync();
                        _consoleUI.Pause();
                        break;
                    case MenuOptions.ViewAllContacts:
                        await ViewAllContactsFlowAsync();
                        _consoleUI.Pause();
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

        private async Task DeleteContactFlowAsync()
        {
            var firstName = AnsiConsole.Ask<string>("Enter [green]first name[/]:");
            var lastName = AnsiConsole.Ask<string>("Enter [green]last name[/]:");

            var matches = await _contactController.SearchByNameAsync(firstName, lastName);

            if (matches.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No matching contact found.[/]");
                return;
            }

            Contact contactToDelete;

            if (matches.Count == 1)
            {
                contactToDelete = matches[0];
            }
            else
            {
                AnsiConsole.MarkupLine($"[yellow]{matches.Count} contacts found with that name:[/]");
                contactToDelete = AnsiConsole.Prompt(
                    new SelectionPrompt<Contact>()
                        .Title("Select the contact to delete:")
                        .UseConverter(c => $"Id: {c.Id} | {c.OrganizationName ?? "-"}")
                        .AddChoices(matches));
            }

            var confirm = AnsiConsole.Confirm(
                $"Are you sure you want to delete [red]{contactToDelete.FirstName} {contactToDelete.LastName}[/]?", false);

            if (!confirm)
            {
                AnsiConsole.MarkupLine("[grey]Delete cancelled.[/]");
                return;
            }

            await _contactController.DeleteContactAsync(contactToDelete.Id);
        }

        private async Task ModifyContactFlowAsync()
        {
            var firstName = AnsiConsole.Ask<string>("Enter [green]first name[/]:");
            var lastName = AnsiConsole.Ask<string>("Enter [green]last name[/]:");

            var matches = await _contactController.SearchByNameAsync(firstName, lastName);

            if (matches.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No matching contact found.[/]");
                return;
            }

            Contact contact;

            if (matches.Count == 1)
            {
                contact = matches[0];
            }
            else
            {
                AnsiConsole.MarkupLine($"[yellow]{matches.Count} contacts found with that name:[/]");
                contact = AnsiConsole.Prompt(
                    new SelectionPrompt<Contact>()
                        .Title("Select the contact to modify:")
                        .UseConverter(c => $"Id: {c.Id} | {c.OrganizationName ?? "-"}")
                        .AddChoices(matches));
            }

            AnsiConsole.MarkupLine("[grey]Current details:[/]");
            // DisplayContact(contact); // if you've built this helper already

            var fieldsToUpdate = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("What would you like to [green]update[/]?")
                    .AddChoices("First Name", "Last Name", "Organization", "Job Title", "Notes", "Phone Numbers", "Emails"));

            if (fieldsToUpdate.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No changes selected.[/]");
                return;
            }

            foreach (var field in fieldsToUpdate)
            {
                switch (field)
                {
                    case "First Name":
                        contact.FirstName = PromptValidated(
                            $"Enter new [green]first name[/] (current: {contact.FirstName}):",
                            InputValidator.IsValidName);
                        break;

                    case "Last Name":
                        contact.LastName = PromptValidated(
                            $"Enter new [green]last name[/] (current: {contact.LastName}):",
                            InputValidator.IsValidName);
                        break;

                    case "Organization":
                        contact.OrganizationName = AnsiConsole.Ask(
                            $"Enter new [green]organization[/] (current: {contact.OrganizationName ?? "-"}):",
                            contact.OrganizationName ?? string.Empty);
                        break;

                    case "Job Title":
                        contact.JobTitle = AnsiConsole.Ask(
                            $"Enter new [green]job title[/] (current: {contact.JobTitle ?? "-"}):",
                            contact.JobTitle ?? string.Empty);
                        break;

                    case "Notes":
                        contact.Notes = AnsiConsole.Ask(
                            $"Enter new [green]notes[/] (current: {contact.Notes ?? "-"}):",
                            contact.Notes ?? string.Empty);
                        break;

                    case "Phone Numbers":
                        contact.PhoneNumbers.Add(new PhoneNumberDetail
                        {
                            Number = PromptValidated("Enter [green]new phone number[/]:", InputValidator.IsValidPhoneNumber),
                            Label = PromptForLabelType()
                        });
                        break;

                    case "Emails":
                        contact.Emails.Add(new EmailDetail
                        {
                            EmailAddress = PromptValidated("Enter [green]new email[/]:", InputValidator.IsValidEmail),
                            Label = PromptForLabelType()
                        });
                        break;
                }
            }
            await _contactController.ModifyContact(contact);
        }

        private async Task ViewContactFlowAsync()
        {
            var firstName = AnsiConsole.Ask<string>("Enter [green]first name[/]:");
            var lastName = AnsiConsole.Ask<string>("Enter [green]last name[/]:");

            var matches = await _contactController.SearchByNameAsync(firstName, lastName);

            if (matches.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No matching contact found.[/]");
                return;
            }

            Contact contact;

            if (matches.Count == 1)
            {
                contact = matches[0];
            }
            else
            {
                AnsiConsole.MarkupLine($"[yellow]{matches.Count} contacts found with that name:[/]");
                contact = AnsiConsole.Prompt(
                    new SelectionPrompt<Contact>()
                        .Title("Select the contact to view:")
                        .UseConverter(c => $"Id: {c.Id} | {c.OrganizationName ?? "-"}")
                        .AddChoices(matches));
            }

            DisplayContacts(new List<Contact> { contact });
        }

        private async Task ViewAllContactsFlowAsync()
        {
            var contacts = await _contactController.ViewAllContacts();
            DisplayContacts(contacts);
        }

        private void DisplayContacts(List<Contact> contacts)
        {
            if (contacts.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No contacts found.[/]");
                return;
            }

            var table = new Table().Border(TableBorder.Rounded);
            table.AddColumn("Id");
            table.AddColumn("Name");
            table.AddColumn("Organization");
            table.AddColumn("Job Title");
            table.AddColumn("Phone(s)");
            table.AddColumn("Email(s)");
            table.AddColumn("Notes");

            foreach (var contact in contacts)
            {
                var phones = string.Join(", ", contact.PhoneNumbers.ConvertAll(p => $"{p.Number} ({p.Label})"));
                var emails = string.Join(", ", contact.Emails.ConvertAll(e => $"{e.EmailAddress} ({e.Label})"));

                table.AddRow(
                    contact.Id.ToString(),
                    $"{contact.FirstName} {contact.LastName}",
                    contact.OrganizationName ?? "-",
                    contact.JobTitle ?? "-",
                    string.IsNullOrEmpty(phones) ? "-" : phones,
                    string.IsNullOrEmpty(emails) ? "-" : emails,
                    contact.Notes ?? "-"
                );
            }

            AnsiConsole.Write(table);
        }

        private LabelType PromptForLabelType()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<LabelType>()
                    .Title("Select [green]label type[/]:")
                    .AddChoices(Enum.GetValues<LabelType>()));
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
