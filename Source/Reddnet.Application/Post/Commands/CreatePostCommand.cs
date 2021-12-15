using MediatR;
using Reddnet.Application.Interfaces;
using Reddnet.Application.Validation;
using Reddnet.Domain.Entities;

namespace Reddnet.Application.Post.Commands;

public record CreatePostCommand : IRequest<Result<PostEntity>>
{
    public Guid CommunityId { get; init; }
    public string Title { get; init; }
    public string Content { get; init; }
    public byte[] Image { get; init; }
}

internal class CreatePostHandler : IRequestHandler<CreatePostCommand, Result<PostEntity>>
{
    private readonly IDataContext context;

    public CreatePostHandler(IDataContext context)
        => this.context = context;

    public async Task<Result<PostEntity>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = this.context.Posts.Add(new()
        {
            Id = Guid.NewGuid(),
            CommunityId = request.CommunityId,
            Title = request.Title,
            Content = request.Content,
            Image = request.Image
        });

        await this.context.SaveChangesAsync(cancellationToken);
        return Result<PostEntity>.Ok(post.Entity);
    }
}
