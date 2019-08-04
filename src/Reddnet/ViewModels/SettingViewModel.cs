using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCoreEngine.ViewModels
{
    public class SettingViewModel
    {
        [Required]
        public string Title { get; set; }

        public IFormFile Logo { get; set; }
    }
}
