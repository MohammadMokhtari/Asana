using Microsoft.AspNetCore.Mvc;

namespace Asana.WebUI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
    }
}
