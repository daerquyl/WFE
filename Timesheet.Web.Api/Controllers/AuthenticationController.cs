﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Timesheet.Infrastructure.Authentication;
using Timesheet.Infrastructure.Authentication.Models;

namespace Timesheet.Web.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService<string> _authenticationService;
        private readonly string _JwtSigningKey ;

        public AuthenticationController(IAuthenticationService<string> authenticationService, IConfiguration configuration)
        {
            this._authenticationService = authenticationService;
            this._JwtSigningKey = configuration.GetSection("AppSettings:Token").Value;
        }

        [HttpPost]
        public IActionResult Login([FromBody] Credentials credentials)
        {
            try
            {
                var token = _authenticationService.LogIn(credentials, _JwtSigningKey);
                return Ok(token);
            }catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
