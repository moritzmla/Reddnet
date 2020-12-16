using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Interfaces;
using Reddnet.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Community.Queries
{
    public class GetAllCommunitiesQuery : IRequest<IEnumerable<CommunityEntity>>
    {
    }

    internal class GetAllSubredditsHandler : IRequestHandler<GetAllCommunitiesQuery, IEnumerable<CommunityEntity>>
    {
        private readonly IDataContext context;

        public GetAllSubredditsHandler(IDataContext context)
            => this.context = context;

        public async Task<IEnumerable<CommunityEntity>> Handle(GetAllCommunitiesQuery request, CancellationToken cancellationToken)
            => await this.context.Communities
                .ToListAsync(cancellationToken);
    }
}
