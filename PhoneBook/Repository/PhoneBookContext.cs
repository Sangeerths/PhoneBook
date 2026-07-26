using Microsoft.EntityFrameworkCore;
using PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneBook.Repository
{
    public class PhoneBookContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PhoneNumberDetail> PhoneNumbers { get; set; }
        public DbSet<EmailDetail> Emails { get; set; }
    }
}
