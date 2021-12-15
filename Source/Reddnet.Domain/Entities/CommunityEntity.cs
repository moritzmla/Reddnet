using Microsoft.EntityFrameworkCore.Infrastructure;
using Reddnet.Domain.Interfaces;

namespace Reddnet.Domain.Entities;

public record CommunityEntity : Entity, IUserEntity, IAuditEntity
{
    private UserEntity _user;
    private ICollection<PostEntity> _posts;
    private readonly ILazyLoader lazyLoader;

    public CommunityEntity() { }
    public CommunityEntity(ILazyLoader lazyLoader)
    {
        this.lazyLoader = lazyLoader;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] Image { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }

    public Guid? UserId { get; set; }
    public UserEntity User
    {
        get => this.lazyLoader.Load(this, ref _user);
        set => _user = value;
    }

    public ICollection<PostEntity> Posts
    {
        get => this.lazyLoader.Load(this, ref _posts);
        set => _posts = value;
    }
}
