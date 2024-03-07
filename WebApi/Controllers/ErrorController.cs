using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Errors;

namespace WebApi.Controllers
{
    [ApiExplorerSettings(GroupName = "APIErrors")]
    [Route("errors")]
    public class ErrorController : BaseApiController
    {
        [HttpGet]
        public IActionResult Error(int code)
        {
            return new ObjectResult(new CodeErrorResponse(code));
        }
    }
}
