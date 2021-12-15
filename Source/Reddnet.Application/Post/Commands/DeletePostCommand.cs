using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Interfaces;
using Reddnet.Application.Validation;

namespace Reddnet.Application.Post.Commands;

public record DeletePostCommand : IRequest<Result>
{
    public Guid Id { get; init; }
}

internal class DeletePostHandler : IRequestHandler<DeletePostCommand, Result>
{
    private readonly IDataContext context;
    private readonly IUser user;

    public DeletePostHandler(IDataContext context, IUser user)
    {
        this.context = context;
        this.user = user;
    }

    public async Task<Result> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await this.context.Posts
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (post is null || (user.IsAuthenticated && post.UserId != user.Id))
        {
            return Result.Failed("Not found");
        }

        this.context.Posts.Remove(post);
        await this.context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
