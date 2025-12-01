using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZealTravel.Infrastructure.FlightObject
{
    public class Client
    {
        public string Title { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Address1 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string PostCode { get; set; }

        public string Street { get; set; }

        public string AreaCode { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string PassportNo { get; set; }

        [DefaultValue(0)]
        public int ClientId { get; set; }

        [DefaultValue(false)]
        public bool Registered { get; set; }

        public string DOB { get; set; }

        public string Password { get; set; }

        public Client()
        {
            ClientId = 0;
            Registered = false;
            Title = (FirstName = (MiddleName = (LastName = (Address = (Address1 = (City = (State = (Country = (PostCode = (Mobile = (Email = (Street = (PassportNo = (AreaCode = (DOB = (Password = ""))))))))))))))));
        }
    }
}