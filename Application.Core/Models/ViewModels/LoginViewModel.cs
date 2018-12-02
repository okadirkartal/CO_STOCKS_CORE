using System.ComponentModel.DataAnnotations;

namespace Application.Core.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(20,ErrorMessage = "Username can be maximum 20 length")]
        public string username { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Password can be maximum 20 length")]
        public string password { get; set; }
    }
}