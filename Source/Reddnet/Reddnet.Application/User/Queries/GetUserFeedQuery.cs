using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Interfaces;
using Reddnet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.User.Queries
{
    public class GetUserFeedQuery : IRequest<IEnumerable<PostEntity>>
    {
        public Guid Id { get; set; }
    }

    internal class GetUserFeedHandler : IRequestHandler<GetUserFeedQuery, IEnumerable<PostEntity>>
    {
        private readonly IDataContext context;

        public GetUserFeedHandler(IDataContext context)
            => this.context = context;

        public async Task<IEnumerable<PostEntity>> Handle(GetUserFeedQuery request, CancellationToken cancellationToken)
            => await this.context.Posts.ToListAsync(cancellationToken);
    }
}
