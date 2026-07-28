using PhoneBook.Models;
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
        internal async Task InsertContactAsync(Contact contact)
        {
            try
            {
                await _contactService.InsertContactAsync(contact);
            }
            catch (Exception ex)
            {
                _consoleUI.ShowError(ex.Message);
                _consoleUI.Pause();
            }
        }

        internal async Task DeleteContactAsync(int id)
        {
            try
            {
               await _contactService.DeleteContactAsync(id);
            }
            catch (Exception ex)
            {
                _consoleUI.ShowError(ex.Message);
                _consoleUI.Pause();
            }
        }

        internal async Task ModifyContact(Contact contact)
        {
            try
            {
               await _contactService.ModifyContactAsync(contact);
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

        internal async Task<List<Contact>> ViewAllContacts()
        {
            try
            {
                return await _contactService.ViewAllContactAsync();
            }
            catch (Exception ex)
            {
                _consoleUI.ShowError(ex.Message);
                _consoleUI.Pause();
                return new List<Contact>();
            }
        }

        internal async Task<List<Contact>> SearchByNameAsync(string firstName, string lastName)
        {
            try
            {
                return await _contactService.SearchByNameAsync(firstName, lastName);
            }
            catch (Exception ex)
            {
                _consoleUI.ShowError(ex.Message);
                _consoleUI.Pause();
                return new List<Contact>();
            }
        }
    }
}
