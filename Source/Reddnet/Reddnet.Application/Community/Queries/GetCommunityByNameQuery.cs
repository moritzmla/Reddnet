using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Exceptions;
using Reddnet.Application.Interfaces;
using Reddnet.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Community.Queries
{
    public record GetCommunityByNameQuery : IRequest<CommunityEntity>
    {
        public string Name { get; init; }
    }

    internal class GetSubredditByNameHandler : IRequestHandler<GetCommunityByNameQuery, CommunityEntity>
    {
        private readonly IDataContext context;

        public GetSubredditByNameHandler(IDataContext context)
            => this.context = context;

        public async Task<CommunityEntity> Handle(GetCommunityByNameQuery request, CancellationToken cancellationToken)
        {
            var community = await this.context.Communities
                .FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);

            if (community is null)
            {
                throw new NotFoundException();
            }

            return community;
        }
    }
}
