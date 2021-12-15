using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Interfaces;
using Reddnet.Application.Validation;

namespace Reddnet.Application.Reply.Commands;

public record DeleteReplyCommand : IRequest<Result>
{
    public Guid Id { get; init; }
}

internal class DeleteReplyHandler : IRequestHandler<DeleteReplyCommand, Result>
{
    private readonly IDataContext context;
    private readonly IUser user;

    public DeleteReplyHandler(IDataContext context, IUser user)
    {
        this.context = context;
        this.user = user;
    }

    public async Task<Result> Handle(DeleteReplyCommand request, CancellationToken cancellationToken)
    {
        var reply = await this.context.Replies
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (reply is null || (user.IsAuthenticated && reply.UserId != user.Id))
        {
            return Result.Failed("Not found");
        }

        this.context.Replies.Remove(reply);
        await this.context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
