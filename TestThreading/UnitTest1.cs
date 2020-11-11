using Microsoft.VisualStudio.TestTools.UnitTesting;
using AddressBookThreading;
using System.Collections.Generic;

namespace TestThreading
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// UC 21
        /// Adds Multiple contacts to database using multi threading
        /// </summary>
        [TestMethod]
        public void InsertMultipleContacts()
        {
            ///Arrange
            AddressBookRepository addressBookRepository = new AddressBookRepository();
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
                AddressBookName = "Mukhesh",
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
                ContactType = "Family"
            });
            // Act
            addressBookRepository.InsertMultipleContactsWithThreads(contactsList);
            List<ContactDetails> actual = addressBookRepository.GetAddressBookDetails().FindAll(contact => (contact.FirstName == "Manish" && contact.LastName == "Pandey") ||
                                                                                           (contact.FirstName == "Vijay" && contact.LastName == "Sankar") ||
                                                                                           (contact.FirstName == "Rashid" && contact.LastName == "Khan"));
            ///Assert
            CollectionAssert.AreEqual(actual, contactsList);
        }
    }
}
