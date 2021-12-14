namespace Reddnet.Domain.Entities;

public record Entity
{
    public Guid Id { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }
}
