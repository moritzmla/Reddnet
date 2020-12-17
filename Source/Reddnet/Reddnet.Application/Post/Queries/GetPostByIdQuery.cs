using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Exceptions;
using Reddnet.Application.Interfaces;
using Reddnet.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Post.Queries
{
    public record GetPostByIdQuery : IRequest<PostEntity>
    {
        public Guid Id { get; init; }
    }

    internal class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, PostEntity>
    {
        private readonly IDataContext context;

        public GetPostByIdHandler(IDataContext context)
            => this.context = context;

        public async Task<PostEntity> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await this.context.Posts
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (post is null)
            {
                throw new NotFoundException();
            }

            return post;
        }
    }
}
