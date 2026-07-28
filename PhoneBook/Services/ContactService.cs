using PhoneBook.Models;
using PhoneBook.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.Services
{
    public class ContactService
    {
        private readonly PhoneBookContext _phoneBookContext;

        public ContactService()
        {
            _phoneBookContext = new PhoneBookContext();
        }
        internal async Task DeleteContactAsync()
        {
            
        }

        internal async Task InsertContactAsync(Contact contact)
        {
            _phoneBookContext.Contacts.Add(contact);
            await _phoneBookContext.SaveChangesAsync();
        }

        internal void ModifyContactAsync()
        {
            throw new NotImplementedException();
        }

        internal void ViewAllContactAsync()
        {
            throw new NotImplementedException();
        }

        internal void ViewContactAsync()
        {
            throw new NotImplementedException();
        }
    }
}
