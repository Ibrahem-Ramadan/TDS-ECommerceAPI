using ECommerceAPI.Models;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthenticationController(IAuthService authService)
        {
                _authService = authService;
        }

        // POST api/<AuthenticationController>
        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var authModel = await _authService.RegisterAsync(model);

            if (!authModel.IsAuthenticated)
                return BadRequest(authModel.Message);

            return Ok(authModel);

        }
        // POST api/<AuthenticationController>
        [HttpPost("register-Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authModel = await _authService.RegisterAdminAsync(model);

            if (!authModel.IsAuthenticated)
                return BadRequest(authModel.Message);

            return Ok(authModel);

        }
        // POST api/<AuthenticationController>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authModel = await _authService.LoginAsync(model);

            if (!authModel.IsAuthenticated)
                return BadRequest(authModel.Message);

            return Ok(authModel);

        }

    }
}
