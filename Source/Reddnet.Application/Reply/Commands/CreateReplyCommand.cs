using MediatR;
using Reddnet.Application.Interfaces;
using Reddnet.Application.Validation;
using Reddnet.Domain.Entities;

namespace Reddnet.Application.Reply.Commands;

public record CreateReplyCommand : IRequest<Result<ReplyEntity>>
{
    public Guid PostId { get; init; }
    public string Content { get; init; }
}

internal class CreateReplyHandler : IRequestHandler<CreateReplyCommand, Result<ReplyEntity>>
{
    private readonly IDataContext context;

    public CreateReplyHandler(IDataContext context)
        => this.context = context;

    public async Task<Result<ReplyEntity>> Handle(CreateReplyCommand request, CancellationToken cancellationToken)
    {
        var reply = this.context.Replies.Add(new()
        {
            Id = Guid.NewGuid(),
            PostId = request.PostId,
            Content = request.Content
        });

        await this.context.SaveChangesAsync(cancellationToken);
        return Result<ReplyEntity>.Ok(reply.Entity);
    }
}
