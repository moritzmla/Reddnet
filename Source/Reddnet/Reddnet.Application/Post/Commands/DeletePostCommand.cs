using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Exceptions;
using Reddnet.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Post.Commands
{
    public class DeletePostCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    internal class DeletePostHandler : AsyncRequestHandler<DeletePostCommand>
    {
        private readonly IDataContext context;
        private readonly ICurrentUserAccessor userAccessor;

        public DeletePostHandler(IDataContext context, ICurrentUserAccessor userAccessor)
        {
            this.context = context;
            this.userAccessor = userAccessor;
        }

        protected override async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await this.context.Posts
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (post is null || (userAccessor.IsAuthenticated && post.UserId != userAccessor.Id))
            {
                throw new NotFoundException();
            }

            this.context.Posts.Remove(post);
            await this.context.SaveChangesAsync(cancellationToken);
        }
    }
}
