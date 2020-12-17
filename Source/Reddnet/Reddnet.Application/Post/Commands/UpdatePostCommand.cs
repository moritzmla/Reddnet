using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Exceptions;
using Reddnet.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Post.Commands
{
    public record UpdatePostCommand : IRequest<Guid>
    {
        public Guid Id { get; init; }
        #pragma warning disable CS8632
        public byte[]? Image { get; init; }
        public string? Title { get; init; }
        public string? Content { get; init; }
        #pragma warning restore CS8632
    }

    internal class UpdatePostHandler : IRequestHandler<UpdatePostCommand, Guid>
    {
        private readonly IDataContext context;
        private readonly ICurrentUserAccessor userAccessor;

        public UpdatePostHandler(IDataContext context, ICurrentUserAccessor userAccessor)
        {
            this.context = context;
            this.userAccessor = userAccessor;
        }

        public async Task<Guid> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await this.context.Posts
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (post == null || (userAccessor.IsAuthenticated && post.UserId != userAccessor.Id))
            {
                throw new NotFoundException();
            }

            if (request.Image != null)
            {
                post.Image = request?.Image;
            }

            if (request.Title != null)
            {
                post.Title = request?.Title;
            }

            if (request.Content != null)
            {
                post.Content = request?.Content;
            }

            await this.context.SaveChangesAsync(cancellationToken);

            return post.Id;
        }
    }
}
