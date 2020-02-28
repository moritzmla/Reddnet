using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Reddnet.ViewModels
{
    public class SettingViewModel
    {
        [Required]
        public string Title { get; set; }

        public IFormFile Logo { get; set; }
    }
}
