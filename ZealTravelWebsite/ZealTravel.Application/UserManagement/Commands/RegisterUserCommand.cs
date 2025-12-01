using System.ComponentModel.DataAnnotations;

namespace ZealTravel.Application.UserManagement.Commands
{
    public class RegisterUserCommand
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int PostalCode { get; set; }
        public string GST { get; set; }
        public string PanNo { get; set; }
        public string Pan_Name { get; set; }
        public string Company_Mobile { get; set; }
        public string Company_PhoneNo { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string pwd { get; set; }
        public string Host { get; set; }
        public string? UserType { get; set; }
        public string? StaffType { get; set; }



    }
}

