using System.ComponentModel.DataAnnotations;

namespace BlogCoreEngine.Web.ViewModels
{
    public class BlogViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public byte[] Cover { get; set; }
    }
}
