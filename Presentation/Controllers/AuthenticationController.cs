using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _service;

        public AuthenticationController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]   // Check Null values that come from parametres.
        public async Task<IActionResult> RegisterUser([FromBody]UserForRegistrationDto userForRegistrationDto)
        {
            var result = await _service.AuthenticationService.RegisterUser(userForRegistrationDto);

            if(result.Succeeded is true)
            {
                return StatusCode(201); 
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }


        }

    }
}
