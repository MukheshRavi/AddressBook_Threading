using Microsoft.VisualStudio.TestTools.UnitTesting;
using AddressBookThreading;
using System.Collections.Generic;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestThreading
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// Calling RestClient class
        /// </summary>
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
        }
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
        /// <summary>
        /// UC 22:
        /// Setting up the method to get all the contacts.
        /// </summary>
        /// <returns></returns>
        private IRestResponse GetContactList()
        {
            //Arrange
            RestRequest request = new RestRequest("/contacts", Method.GET);

            //Act
            IRestResponse response = client.Execute(request);
            return response;
        }
        [TestMethod]
        public void OnCallingGETApi_ShouldReturnContactList()
        {
            IRestResponse response = GetContactList();

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<ContactDetails> dataResponse = JsonConvert.DeserializeObject<List<ContactDetails>>(response.Content);
            Assert.AreEqual(3, dataResponse.Count);
            foreach (ContactDetails contact in dataResponse)
            {
                System.Console.WriteLine("FirstName: " + contact.FirstName + " LastName: " + contact.LastName + " PhoneNumber" + contact.PhoneNumber);
            }
        }
        /// <summary>
        /// UC 23:
        /// POST api will add multiple contacts to the json file created
        /// </summary>
        [TestMethod]
        public void OnCallingPOSTApi_ShouldAddMultipleContacts()
        {
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

            });
            contactsList.ForEach(contact =>
            {
                //Arrange
                RestRequest request = new RestRequest("/contacts", Method.POST);
                JObject jObjectBody = new JObject();
                jObjectBody.Add("FirstName", contact.FirstName);
                jObjectBody.Add("LastName", contact.LastName);
                jObjectBody.Add("PhoneNumber", contact.PhoneNumber);
                jObjectBody.Add("City", contact.City);
                jObjectBody.Add("State", contact.State);
                jObjectBody.Add("Email", contact.Email);
                jObjectBody.Add("Country", contact.Country);
                request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

                //Act
                IRestResponse response = client.Execute(request);
                ContactDetails dataResponse = JsonConvert.DeserializeObject<ContactDetails>(response.Content);
                //Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                Assert.AreEqual(contact.FirstName, dataResponse.FirstName);
                Assert.AreEqual(contact.LastName, dataResponse.LastName);
                System.Console.WriteLine(response.Content);
            });
        }
        // <summary>
        /// UC 24:
        /// PUT api will update contact FirstName and State in the json file
        /// </summary>
        [TestMethod]
        public void OnCallingPUTApi_ShouldUpdateContact()
        {
            //Arrange
            RestRequest request = new RestRequest("/contacts/1", Method.PUT);
            JObject jObjectBody = new JObject();
            jObjectBody.Add("FirstName","Mukhesh");
            jObjectBody.Add("LastName","Attuluri");
            jObjectBody.Add("PhoneNumber", "8978496720");
            jObjectBody.Add("City", "Ppm");
            jObjectBody.Add("State", "Ap");
            jObjectBody.Add("Email", "mkh.com");
            jObjectBody.Add("Country", "India");

            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            ContactDetails dataResponse = JsonConvert.DeserializeObject<ContactDetails>(response.Content);
            Assert.AreEqual("Mukhesh", dataResponse.FirstName);
            Assert.AreEqual("Attuluri", dataResponse.LastName);
            System.Console.WriteLine(response.Content);
        }
        /// <summary>
        /// UC 25:
        /// DELETE api will delete contact in the json file
        /// </summary>
        [TestMethod]
        public void OnCallingDELETEApi_ShouldDeleteContact()
        {
            //Arrange
            RestRequest request = new RestRequest("/contacts/2", Method.DELETE);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            System.Console.WriteLine(response.Content);
        }
    }
}
