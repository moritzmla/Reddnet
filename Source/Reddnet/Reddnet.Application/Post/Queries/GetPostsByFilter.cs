using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Interfaces;
using Reddnet.Application.Validation;
using Reddnet.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Post.Queries
{
    public record GetPostsByFilter : IRequest<Result<IEnumerable<PostEntity>>>
    {
        public string Query { get; init; }
    }

    internal class GetPostsByFilterHandler : IRequestHandler<GetPostsByFilter, Result<IEnumerable<PostEntity>>>
    {
        private readonly IDataContext context;

        public GetPostsByFilterHandler(IDataContext context)
            => this.context = context;

        public async Task<Result<IEnumerable<PostEntity>>> Handle(GetPostsByFilter request, CancellationToken cancellationToken)
        {
            var posts = string.IsNullOrWhiteSpace(request.Query) ?
                default
                : await this.context.Posts
                #pragma warning disable RCS1155
                .Where(x => x.Title.ToLower().Contains(request.Query.ToLower()))
                #pragma warning restore RCS1155
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<PostEntity>>.Ok(posts);
        }
    }
}
