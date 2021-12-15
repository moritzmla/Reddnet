using Microsoft.EntityFrameworkCore.Infrastructure;
using Reddnet.Domain.Interfaces;

namespace Reddnet.Domain.Entities;

public record ReplyEntity : Entity, IUserEntity, IAuditEntity
{
    private UserEntity _user;
    private PostEntity _post;
    private readonly ILazyLoader lazyLoader;

    public ReplyEntity() { }
    public ReplyEntity(ILazyLoader lazyLoader)
    {
        this.lazyLoader = lazyLoader;
    }

    public string Content { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }

    public Guid? UserId { get; set; }
    public UserEntity User
    {
        get => this.lazyLoader.Load(this, ref _user);
        set => _user = value;
    }

    public Guid? PostId { get; set; }
    public PostEntity Post
    {
        get => this.lazyLoader.Load(this, ref _post);
        set => _post = value;
    }
}
