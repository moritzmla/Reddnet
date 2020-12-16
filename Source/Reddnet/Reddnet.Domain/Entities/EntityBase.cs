using System;

namespace Reddnet.Domain.Entities
{
    public class EntityBase
    {
        public Guid Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }
    }
}
