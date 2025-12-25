using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Kế thừa các trường có sẵn trong identyty

        // Thêm các trường riêng của dự án TechZone:
        [Required]
        public string FullName { get; set; }

        public string? Address { get; set; }

        public DateTime? Dob { get; set; }
    }
}