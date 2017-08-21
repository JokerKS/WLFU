using System.ComponentModel.DataAnnotations;

namespace WLFU.Models
{
    public class LoginModel
    {
        [Required]
        public string EmailOrUserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}