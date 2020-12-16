using MediatR;
using Reddnet.Application.Interfaces;
using Reddnet.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reddnet.Application.Post.Commands
{
    public class CreatePostCommand : IRequest<PostEntity>
    {
        public Guid CommunityId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public byte[] Image { get; set; }
    }

    internal class CreatePostHandler : IRequestHandler<CreatePostCommand, PostEntity>
    {
        private readonly IDataContext context;

        public CreatePostHandler(IDataContext context)
            => this.context = context;

        public async Task<PostEntity> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = this.context.Posts.Add(new PostEntity
            {
                Id = Guid.NewGuid(),
                CommunityId = request.CommunityId,
                UserId = request.UserId,
                Title = request.Title,
                Content = request.Content,
                Image = request.Image
            });

            await this.context.SaveChangesAsync(cancellationToken);
            return post.Entity;
        }
    }
}
