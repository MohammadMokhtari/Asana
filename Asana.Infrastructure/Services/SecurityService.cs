using System;
using System.Security.Cryptography;

namespace Asana.Infrastructure.Services
{
    public interface ISecurityService
    {
        public Guid CreateCryptographicallySecureGuid();
    }

    public class SecurityService : ISecurityService
    {
        public Guid CreateCryptographicallySecureGuid()
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[16];
                provider.GetBytes(bytes);

                return new Guid(bytes);
            }
        }
    }
}
