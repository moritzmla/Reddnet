using System;

namespace Reddnet.Application.Interfaces
{
    public interface ICurrentUserAccessor
    {
        public Guid Id { get; }
        public string UserName { get; }
        public bool IsAuthenticated { get; }
    }
}
