using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCoreEngine.ViewModels
{
    public class ProfileViewModel
    {
        public IFormFile ProfilePicture { get; set; }
    }
}
