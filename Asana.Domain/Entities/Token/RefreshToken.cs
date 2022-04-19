using Asana.Domain.Entities.Common;
using System;

namespace Asana.Domain.Entities.Token
{
    public class RefreshToken : BaseEntity, ISoftDeletable
    {
        public string Token { get; set; }

        public string JwtId { get; set; }

        public DateTimeOffset RefreshTokenExpiresDate { get; set; }

        public bool IsDelete { get; set; }

        public bool Used { get; set; }

        public bool Invalidated { get; set; }

        public Guid UserId { get; set; }

    }
}
