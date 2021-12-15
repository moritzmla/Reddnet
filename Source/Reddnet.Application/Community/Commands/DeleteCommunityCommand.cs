using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Interfaces;
using Reddnet.Application.Validation;

namespace Reddnet.Application.Community.Commands;

public record DeleteCommunityCommand : IRequest<Result>
{
    public string Name { get; init; }
}

internal class DeleteSubredditHandler : IRequestHandler<DeleteCommunityCommand, Result>
{
    private readonly IDataContext context;
    private readonly IUser user;

    public DeleteSubredditHandler(IDataContext context, IUser user)
    {
        this.context = context;
        this.user = user;
    }

    public async Task<Result> Handle(DeleteCommunityCommand request, CancellationToken cancellationToken)
    {
        var community = await this.context.Communities
            .FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);

        if (community is null || (user.IsAuthenticated && community.UserId != user.Id))
        {
            return Result.Failed("Not found");
        }

        foreach (var post in community.Posts)
        {
            this.context.Posts.Remove(post);
        }

        this.context.Communities.Remove(community);
        await this.context.SaveChangesAsync(cancellationToken);
        return Result.Ok();
    }
}
