using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Nowcfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
        }

        internal IActionResult Ok<T>(T content)
        {
            return base.Ok(content);
        }

        internal IActionResult NotFound(string message)
        {
            return base.NotFound(HandleActionResult(message, StatusCodes.Status404NotFound));
        }

        internal IActionResult UnAuthorized(string message="")
        {
            return Unauthorized(HandleActionResult(message, StatusCodes.Status401Unauthorized));
        }

        internal IActionResult BadRequest(string message="",Object value=null)
        {
            return base.BadRequest(HandleActionResult(message, StatusCodes.Status400BadRequest));
        }

        internal IActionResult ExceptionResponse(string message, Object value= null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,HandleActionResult(message, StatusCodes.Status500InternalServerError,value));
        }


        /// <summary>
        /// Gets the action result value and returns it in desired format
        /// </summary>
        /// <param name="errorMessage">error message</param>
        /// <param name="statusCode">http status code</param>
        /// <param name="value"></param>
        /// <returns>new object</returns>
        protected object HandleActionResult(string errorMessage, int statusCode,Object value=null)
        {
            return new { statusCode, errorMessage,value };
        }
    }
}