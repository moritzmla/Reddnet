using System;
using System.ComponentModel.DataAnnotations;

namespace BlogCoreEngine.Core.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
