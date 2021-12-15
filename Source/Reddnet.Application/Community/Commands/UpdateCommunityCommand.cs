using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Interfaces;
using Reddnet.Application.Validation;

namespace Reddnet.Application.Community.Commands;

public record UpdateCommunityCommand : IRequest<Result<string>>
{
    public string Name { get; init; }
#pragma warning disable CS8632
    public string? Description { get; init; }
    public byte[]? Image { get; init; }
#pragma warning restore CS8632
}

internal class UpdateSubredditHandler : IRequestHandler<UpdateCommunityCommand, Result<string>>
{
    private readonly IDataContext context;
    private readonly IUser user;

    public UpdateSubredditHandler(IDataContext context, IUser user)
    {
        this.context = context;
        this.user = user;
    }

    public async Task<Result<string>> Handle(UpdateCommunityCommand request, CancellationToken cancellationToken)
    {
        var community = await this.context.Communities
            .FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);

        if (community == null || (user.IsAuthenticated && community.UserId != user.Id))
        {
            return Result<string>.Failed("Not found");
        }

        if (request.Description != null)
        {
            community.Description = request?.Description;
        }

        if (request.Image != null)
        {
            community.Image = request?.Image;
        }

        await this.context.SaveChangesAsync(cancellationToken);
        return Result<string>.Ok(community.Name);
    }
}
