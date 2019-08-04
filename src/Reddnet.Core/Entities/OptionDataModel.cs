using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
