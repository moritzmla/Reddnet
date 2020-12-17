using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Interfaces;
using Reddnet.Application.Validation;
using Reddnet.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Post.Queries
{
    public record GetPostByIdQuery : IRequest<Result<PostEntity>>
    {
        public Guid Id { get; init; }
    }

    internal class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, Result<PostEntity>>
    {
        private readonly IDataContext context;

        public GetPostByIdHandler(IDataContext context)
            => this.context = context;

        public async Task<Result<PostEntity>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await this.context.Posts
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (post is null)
            {
                return Result<PostEntity>.Failed("Not found");
            }

            return Result<PostEntity>.Ok(post);
        }
    }
}
