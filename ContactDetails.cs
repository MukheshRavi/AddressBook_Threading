using System;
using System.Collections.Generic;
using System.Text;

namespace AddressBookThreading
{
   public  class ContactDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime date { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string AddressBookName { get; set; }
        public string ContactType { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is ContactDetails))
                return false;
            ContactDetails contact = (ContactDetails)obj;
            return this.FirstName == contact.FirstName && this.LastName == contact.LastName;
        }
    }
}
