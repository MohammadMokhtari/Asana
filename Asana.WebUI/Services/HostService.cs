using Asana.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Asana.WebUI.Services
{
    public class HostService : IHostService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HostService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string HostName => _httpContextAccessor.HttpContext?.Request.Host.Value;
    }
}
