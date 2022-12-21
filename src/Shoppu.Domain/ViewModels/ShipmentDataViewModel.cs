using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Shoppu.Domain.Enums;

namespace Shoppu.Domain.ViewModels
{
    public class ShipmentDataViewModel
    {
        [Required(ErrorMessage = "Please enter your first name.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter your last name.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter your city name.")]
        public string City { get; set; }
        [Required(ErrorMessage = "Please enter your postal code.")]
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Please enter your street name.")]
        [Display(Name = "Street name")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Please enter your apartment information.")]
        [Display(Name = "House number")]
        public string Apartment { get; set; }
        [Phone]
        [Required(ErrorMessage = "Please enter your phone number.")]
        [Display(Name = "Phone number")]
        public string Phone { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Please enter an e-mail address.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Select payment method.")]
        [Display(Name = "Payment method")]
        [Range(0, (int)PaymentMethod.OnlineTransfer)]
        public PaymentMethod PaymentMethod { get; set; }

    }
}