using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Interfaces;
using Reddnet.Application.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Post.Commands
{
    public record DeletePostCommand : IRequest<Result>
    {
        public Guid Id { get; init; }
    }

    internal class DeletePostHandler : IRequestHandler<DeletePostCommand, Result>
    {
        private readonly IDataContext context;
        private readonly ICurrentUserAccessor userAccessor;

        public DeletePostHandler(IDataContext context, ICurrentUserAccessor userAccessor)
        {
            this.context = context;
            this.userAccessor = userAccessor;
        }

        public async Task<Result> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await this.context.Posts
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (post is null || (userAccessor.IsAuthenticated && post.UserId != userAccessor.Id))
            {
                return Result.Failed("Not found");
            }

            this.context.Posts.Remove(post);
            await this.context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
