using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Interfaces;
using Reddnet.Application.Validation;
using Reddnet.Domain.Entities;

namespace Reddnet.Application.User.Queries;

public record GetUserFeedQuery : IRequest<Result<IEnumerable<PostEntity>>>
{
    public Guid Id { get; init; }
}

internal class GetUserFeedHandler : IRequestHandler<GetUserFeedQuery, Result<IEnumerable<PostEntity>>>
{
    private readonly IDataContext context;

    public GetUserFeedHandler(IDataContext context)
        => this.context = context;

    public async Task<Result<IEnumerable<PostEntity>>> Handle(GetUserFeedQuery request, CancellationToken cancellationToken)
    {
        var posts = await this.context.Posts.ToListAsync(cancellationToken);
        return Result<IEnumerable<PostEntity>>.Ok(posts);
    }
}
