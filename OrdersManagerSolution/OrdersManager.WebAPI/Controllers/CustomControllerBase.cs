using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OrdersManager.WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/orders")]
    [ApiController]
    public class CustomControllerBase : ControllerBase
    {
    }
}
