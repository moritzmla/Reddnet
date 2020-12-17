using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Interfaces;
using Reddnet.Application.Validation;
using Reddnet.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Community.Queries
{
    public record GetAllCommunitiesQuery : IRequest<Result<IEnumerable<CommunityEntity>>> { }

    internal class GetAllSubredditsHandler : IRequestHandler<GetAllCommunitiesQuery, Result<IEnumerable<CommunityEntity>>>
    {
        private readonly IDataContext context;

        public GetAllSubredditsHandler(IDataContext context)
            => this.context = context;

        public async Task<Result<IEnumerable<CommunityEntity>>> Handle(GetAllCommunitiesQuery request, CancellationToken cancellationToken)
        {
            var communities = await this.context.Communities.ToListAsync(cancellationToken);
            return Result<IEnumerable<CommunityEntity>>.Ok(communities);
        }
    }
}
