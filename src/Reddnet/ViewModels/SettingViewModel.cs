using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlogCoreEngine.ViewModels
{
    public class SettingViewModel
    {
        [Required]
        public string Title { get; set; }

        public IFormFile Logo { get; set; }
    }
}
