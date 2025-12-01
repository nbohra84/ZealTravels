namespace ZealTravel.Backoffice.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CompanyProfiles
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        // public string? Pwd { get; set; }
        public string AccountIdValue { get; set; }

        [Required(ErrorMessage = "User type is required.")]
        public string? UserType { get; set; }

        public bool? AccessStatus { get; set; }

        public bool? ActiveStatus { get; set; }

        [Required(ErrorMessage = "Account ID is required")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Company ID is required.")]
        public string CompanyId { get; set; }

        public string? CompanyLogo { get; set; }
        public string? dop { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        public string? CompanyName { get; set; }

        public string? AdminId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters.")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Middle name can't be longer than 50 characters.")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }

        public string? State { get; set; }

        public List<SelectListItem> States { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Cities { get; set; } = new List<SelectListItem>();

        public string? Country { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Pin code is required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid Pin code. It must be a 6-digit number.")]
        public int? PostalCode { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^(\+?\d{1,3}[-\s]?)?(\(?\d{3}\)?[-\s]?)?\d{3}[-\s]?\d{4}$", ErrorMessage = "Invalid mobile number.")]
        [Phone(ErrorMessage = "Invalid mobile number.")]
        public string? Mobile { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Minimum 10 characters allowed.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Not a valid integer.")]
        public string? PhoneNo { get; set; }

        [EmailAddress(ErrorMessage = "Invalid company email address.")]
        public string? CompanyEmail { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Minimum 10 characters allowed.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Not a valid integer.")]
        public string? CompanyMobile { get; set; }

        [Required(ErrorMessage = "Business Currency is required")]
        public string? Currency { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Minimum 10 characters allowed.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Not a valid integer.")]
        public string? CompanyPhoneNo { get; set; }

        [Phone(ErrorMessage = "Invalid fax number.")]
        public string? FaxNo { get; set; }

        public string? StaxNo { get; set; }

        public decimal? TdsAmount { get; set; }

        public decimal? TdsExemption { get; set; }

        public string? Host { get; set; }

        public string? AbtaNo { get; set; }

        public string? IataNo { get; set; }

        public string? IataMemberSinceYrs { get; set; }

        public string? AtolNo { get; set; }

        public string? TtaNo { get; set; }

        public string? GtgNo { get; set; }

        public string? VatNo { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "No Special character allowed, if required use only space.")]
        public string? PanName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}$", ErrorMessage = "Please enter valid Pan Number.")]
        public string? PanNo { get; set; }

        public string? TanNo { get; set; }

        public string? NtnNo { get; set; }

        public string? CnpjNo { get; set; }

        public string? ServiceTaxNo { get; set; }

        public string? TpinNo { get; set; }

        public string? LicenseNo { get; set; }

        public string? GstName { get; set; }

        [RegularExpression(@"^\d{2}[A-Za-z]{5}\d{4}[A-Za-z]{1}\d[Zz]{1}[A-Za-z\d]{1}$", ErrorMessage = "Please enter a valid GST number.")]
        public string? Gst { get; set; }

        public bool? SafiCharge { get; set; }

        public bool? DirectDebitAgent { get; set; }

        public bool? SelfBilling { get; set; }

        public int? YrsInBusiness { get; set; }

        public int? TotalEmployee { get; set; }

        public int? TotalBranches { get; set; }

        public int? AnnualTurnover { get; set; }

        public int? MonthlyBookingVolume { get; set; }

        public string? BusinessType { get; set; }

        public string? OfficeSpace { get; set; }

        public string? ReferredBy { get; set; }

        public bool? CorporateAgent { get; set; }

        public bool? WhitelabelAgent { get; set; }

        public bool? DistributorAgent { get; set; }

        public string? Reference { get; set; }

        public string? Consolidators { get; set; }

        public string? Remarks { get; set; }

        public int? StaffType { get; set; }

        public string? Ip { get; set; }

        public DateTime? EventTime { get; set; }

        public bool? Fdfares { get; set; }

        public int? Fdmarkup { get; set; }

        public string? UpdateBy { get; set; }

        //[Required(ErrorMessage = "Logo file is required.")]
        public IFormFile LogoFile { get; set; }
    }

}
