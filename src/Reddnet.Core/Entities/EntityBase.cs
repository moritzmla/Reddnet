using System;
using System.ComponentModel.DataAnnotations;

namespace Reddnet.Core.Entities
{
    public class EntityBase
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
