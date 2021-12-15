using MediatR;
using Reddnet.Application.Interfaces;
using Reddnet.Application.Validation;
using Reddnet.Domain.Entities;

namespace Reddnet.Application.Community.Commands;

public record CreateCommunityCommand : IRequest<Result<CommunityEntity>>
{
    public string Name { get; init; }
    public string Description { get; init; }
    public byte[] Image { get; init; }
}

internal class CreateSubredditHandler : IRequestHandler<CreateCommunityCommand, Result<CommunityEntity>>
{
    private readonly IDataContext context;

    public CreateSubredditHandler(IDataContext context)
        => this.context = context;

    public async Task<Result<CommunityEntity>> Handle(CreateCommunityCommand request, CancellationToken cancellationToken)
    {
        var community = this.context.Communities.Add(new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Image = request.Image
        });

        await this.context.SaveChangesAsync(cancellationToken);
        return Result<CommunityEntity>.Ok(community.Entity);
    }
}
