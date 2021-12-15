using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Interfaces;
using Reddnet.Application.Validation;
using Reddnet.Domain.Entities;

namespace Reddnet.Application.Post.Queries;

public record GetPostsByUserName : IRequest<Result<IEnumerable<PostEntity>>>
{
    public string UserName { get; init; }
}

internal class GetPostsByUserNameHandler : IRequestHandler<GetPostsByUserName, Result<IEnumerable<PostEntity>>>
{
    private readonly IDataContext context;

    public GetPostsByUserNameHandler(IDataContext context)
        => this.context = context;

    public async Task<Result<IEnumerable<PostEntity>>> Handle(GetPostsByUserName request, CancellationToken cancellationToken)
    {
        var posts = await this.context.Posts
            .Where(x => x.User.UserName == request.UserName)
            .ToListAsync(cancellationToken);

        return Result<IEnumerable<PostEntity>>.Ok(posts);
    }
}
