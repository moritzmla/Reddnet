using MediatR;
using Microsoft.EntityFrameworkCore;
using Reddnet.Application.Exceptions;
using Reddnet.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Reply.Commands
{
    public record DeleteReplyCommand : IRequest
    {
        public Guid Id { get; init; }
    }

    internal class DeleteReplyHandler : AsyncRequestHandler<DeleteReplyCommand>
    {
        private readonly IDataContext context;
        private readonly ICurrentUserAccessor userAccessor;

        public DeleteReplyHandler(IDataContext context, ICurrentUserAccessor userAccessor)
        {
            this.context = context;
            this.userAccessor = userAccessor;
        }

        protected override async Task Handle(DeleteReplyCommand request, CancellationToken cancellationToken)
        {
            var reply = await this.context.Replies
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (reply is null || (userAccessor.IsAuthenticated && reply.UserId != userAccessor.Id))
            {
                throw new NotFoundException();
            }

            this.context.Replies.Remove(reply);
            await this.context.SaveChangesAsync(cancellationToken);
        }
    }
}
