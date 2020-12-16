using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Interfaces;
using Reddnet.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Post.Queries
{
    public class GetPostsByUserName : IRequest<IEnumerable<PostEntity>>
    {
        public string UserName { get; set; }
    }

    internal class GetPostsByUserNameHandler : IRequestHandler<GetPostsByUserName, IEnumerable<PostEntity>>
    {
        private readonly IDataContext context;

        public GetPostsByUserNameHandler(IDataContext context)
            => this.context = context;

        public async Task<IEnumerable<PostEntity>> Handle(GetPostsByUserName request, CancellationToken cancellationToken)
            => await this.context.Posts
                .Where(x => x.User.UserName == request.UserName)
                .ToListAsync(cancellationToken);
    }
}
