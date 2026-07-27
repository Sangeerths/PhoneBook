using PhoneBook.Services;
using PhoneBook.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.Controller
{
    public class ContactController
    {
        private readonly ContactService _contactService;
        private readonly ConsoleUI _consoleUI;

        public ContactController()
        {
            _contactService = new ContactService();
            _consoleUI = new ConsoleUI();
        }
        internal async Task InsertContactAsync()
        {
            try
            {
                _contactService.InsertContactAsync();
            }
            catch (Exception ex)
            {
                _consoleUI.ShowError(ex.Message);
                _consoleUI.Pause();
            }
        }

        internal async Task DeleteContactAsync()
        {
            try
            {
                _contactService.DeleteContactAsync();
            }
            catch (Exception ex)
            {
                _consoleUI.ShowError(ex.Message);
                _consoleUI.Pause();
            }
        }

        internal async Task ModifyContact()
        {
            try
            {
                _contactService.ModifyContactAsync();
            }
            catch (Exception ex)
            {
                _consoleUI.ShowError(ex.Message);
                _consoleUI.Pause();
            }
        }

        internal async Task ViewContact()
        {
            try
            {
                _contactService.ViewContactAsync();
            }
            catch (Exception ex)
            {
                _consoleUI.ShowError(ex.Message);
                _consoleUI.Pause();
            }
        }

        internal async Task ViewAllContacts()
        {
            try
            {
                _contactService.ViewAllContactAsync();
            }
            catch (Exception ex)
            {
                _consoleUI.ShowError(ex.Message);
                _consoleUI.Pause();
            }
        }

    }
}
