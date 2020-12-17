using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Exceptions;
using Reddnet.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Community.Commands
{
    public record UpdateCommunityCommand : IRequest<string>
    {
        public string Name { get; init; }
        #pragma warning disable CS8632
        public string? Description { get; init; }
        public byte[]? Image { get; init; }
        #pragma warning restore CS8632
    }

    internal class UpdateSubredditHandler : IRequestHandler<UpdateCommunityCommand, string>
    {
        private readonly IDataContext context;
        private readonly ICurrentUserAccessor userAccessor;

        public UpdateSubredditHandler(IDataContext context, ICurrentUserAccessor userAccessor)
        {
            this.context = context;
            this.userAccessor = userAccessor;
        }

        public async Task<string> Handle(UpdateCommunityCommand request, CancellationToken cancellationToken)
        {
            var community = await this.context.Communities
                .FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);

            if (community == null || (userAccessor.IsAuthenticated && community.UserId != userAccessor.Id))
            {
                throw new NotFoundException();
            }

            if (request.Description != null)
            {
                community.Description = request?.Description;
            }

            if (request.Image != null)
            {
                community.Image = request?.Image;
            }

            await this.context.SaveChangesAsync(cancellationToken);

            return community.Name;
        }
    }
}
