using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class ClientDto
    {
        [Required(ErrorMessage ="First Name is Required")]
        public string FirstName { get; set; } = "";
        [Required(ErrorMessage ="Last Name is Required")]
        public string LastName { get; set; } = "";
        [Required,EmailAddress]
        public string Email { get; set; } = "";
        [Phone]
        public string? Phone { get; set; }
        public string? Address  {get; set;}
        [Required]
        public string Status { get; set; } = ""; 
    }
}
