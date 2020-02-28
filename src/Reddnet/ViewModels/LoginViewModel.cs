using System.ComponentModel.DataAnnotations;

namespace Reddnet.ViewModels
{
    public class LoginViewModel
    {
        [Required, Display(Name = "UserName / Email")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}
