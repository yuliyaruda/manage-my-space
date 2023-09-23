using System;

namespace ManageMySpace.Common.Auth
{
    public interface IJwtHandler
    {
        JsonWebTocken Create(Guid userId, string role, string email);
    }
}
