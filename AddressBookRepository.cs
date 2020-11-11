using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookThreading
{
   public class AddressBookRepository
    {
        public static string connectionString = @"Server=MUKESH\SQLEXPRESS; Initial Catalog =addressBookService;;Integrated Security=True;Connect Timeout=30;
                       Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
         SqlConnection connection = new SqlConnection(connectionString);
        List<ContactDetails> contactsList = new List<ContactDetails>();
        public List<ContactDetails> GetAddressBookDetails()
        {
            try
            {
                using (connection)
                {
                    string query = @"SELECT CD.* ,A.Area, A.city,A.State,A.Country,CM1.AddressBookName,CM1.ContactType from contactdetails CD inner join AddressDetails A on
                                       CD.FirstName = A.FirstName and CD.LastName=A.LastName
                                      inner join(select AddressBookName,ContactType, FirstName,LastName from BookNameContactType BC 
				                   inner join ContactTypeMap CM on BC.Contactid = CM.NameTypeid) CM1 
                                  on CD.FirstName = CM1.FirstName and CD.LastName=CM1.LastName";

                    SqlCommand command = new SqlCommand(query, connection);
                    this.connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ContactDetails contactDetails = new ContactDetails();
                            contactDetails.FirstName = reader.GetString(0);
                            contactDetails.LastName = reader.GetString(1);
                            contactDetails.PhoneNumber = reader.GetString(2);
                            contactDetails.Email = reader.GetString(3);
                            contactDetails.Area = reader.GetString(5);
                            contactDetails.City = reader.GetString(6);
                            contactDetails.State = reader.GetString(7);
                            contactDetails.Country = reader.GetString(8);
                            contactDetails.AddressBookName = reader.GetString(9);
                            contactDetails.ContactType = reader.GetString(10);

                            Console.WriteLine(contactDetails.FirstName + "  " + contactDetails.LastName + "  " + contactDetails.PhoneNumber + " " + contactDetails.Area + "  " + contactDetails.City
                                + "  " + contactDetails.State + "  " + contactDetails.Country + " " + contactDetails.AddressBookName + " " + contactDetails.ContactType);
                            Console.WriteLine("\n");
                            contactsList.Add(contactDetails);
                        }
                    }

                    else
                    {
                        Console.WriteLine("No data found");
                    }
                    reader.Close();
                    return contactsList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.connection.Close();
            }
        }
        /// <summary>
        /// UC 20 
        /// Add new Contact
        /// </summary>
        /// <param name="contact"></param>
        public void AddNewContact(ContactDetails contact)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                // Open connection
                connection.Open();

                // Declare a command
                SqlCommand command = new SqlCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "dbo.AddNewContact";
                command.Connection = connection;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@FirstName", contact.FirstName);
                command.Parameters.AddWithValue("@LastName", contact.LastName);
                command.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);
                command.Parameters.AddWithValue("@Email", contact.Email);
                command.Parameters.AddWithValue("@Area", contact.Area);
                command.Parameters.AddWithValue("@City", contact.City);
                command.Parameters.AddWithValue("@State", contact.State);
                command.Parameters.AddWithValue("@Country", contact.Country);
                command.Parameters.AddWithValue("@addressBookName", contact.AddressBookName);
                command.Parameters.AddWithValue("@contactType", contact.ContactType);
                var result = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        // <summary>
        /// UC 21 InsertMultipleContactsWithThreads
        /// </summary>
        /// <param name="contactList"></param>
        public void InsertMultipleContactsWithThreads(List<ContactDetails> contactList)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.ForEach(contactList, contact => AddNewContact(contact));
            stopwatch.Stop();
            Console.WriteLine("Time taken for adding multiple contacts is :" + stopwatch.ElapsedMilliseconds);
        }
    }
}
