using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Exceptions;
using Reddnet.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Community.Commands
{
    public record DeleteCommunityCommand : IRequest
    {
        public string Name { get; init; }
    }

    internal class DeleteSubredditHandler : AsyncRequestHandler<DeleteCommunityCommand>
    {
        private readonly IDataContext context;
        private readonly ICurrentUserAccessor userAccessor;

        public DeleteSubredditHandler(IDataContext context, ICurrentUserAccessor userAccessor)
        {
            this.context = context;
            this.userAccessor = userAccessor;
        }

        protected override async Task Handle(DeleteCommunityCommand request, CancellationToken cancellationToken)
        {
            var community = await this.context.Communities
                .FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);

            if (community is null || (userAccessor.IsAuthenticated && community.UserId != userAccessor.Id))
            {
                throw new NotFoundException();
            }

            foreach (var post in community.Posts)
            {
                this.context.Posts.Remove(post);
            }

            this.context.Communities.Remove(community);
            await this.context.SaveChangesAsync(cancellationToken);
        }
    }
}
