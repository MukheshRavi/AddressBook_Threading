﻿using System;
using System.Collections.Generic;

namespace AddressBookThreading
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            List<ContactDetails> contactsList = new List<ContactDetails>();
            contactsList.Add(new ContactDetails
            {
                FirstName = "Manish",
                LastName = "Pandey",
                PhoneNumber = "2344746584",
                Email = "mani.com",
                Area = "maruti colony",
                City = "Warangal",
                State = "AndhraPradesh",
                Country = "India",
                AddressBookName = "My Book",
                ContactType = "Friend"
            });
            contactsList.Add(new ContactDetails
            {
                FirstName = "Vijay",
                LastName = "Sankar",
                PhoneNumber = "2344734597",
                Email = "vij.com",
                Area = "Vijaya colony",
                City = "Khammam",
                State = "AndhraPradesh",
                Country = "India",
                AddressBookName = "Ravi",
                ContactType = "Friend"
            });
            contactsList.Add(new ContactDetails
            {
                FirstName = "Rashid",
                LastName = "Khan",
                PhoneNumber = "4562746584",
                Email = "khan.com",
                Area = "Happy colony",
                City = "Banglore",
                State = "Karnataka",
                Country = "India",
                AddressBookName = "My Book",
                ContactType = "Relative"
            });
            AddressBookRepository addressBookRepository = new AddressBookRepository();
            addressBookRepository.InsertMultipleContactsWithThreads(contactsList);

        }
    }
}

