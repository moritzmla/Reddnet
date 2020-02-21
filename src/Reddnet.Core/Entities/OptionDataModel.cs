using System.ComponentModel.DataAnnotations;

namespace BlogCoreEngine.Core.Entities
{
    public class OptionDataModel : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public byte[] Logo { get; set; }
    }
}
