using Api.Auth.Application.Dtos.Payloads;
using Api.Auth.Application.Interfaces;
using InvestmentManager.Shared.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Auth.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] LoginPayloadDto payload)
    {
        return Ok(await _authService.AuthenticateAsync(payload));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserPayloadDto payload)
    {
        return Ok(await _authService.RegisterAsync(payload));
    }

    [HttpPost("assignRole")]
    [Authorize(Roles = Role.Admin)]
    public async Task<IActionResult> AssignRole([FromBody] AssingRolePayloadDto payload)
    {
        return Ok(await _authService.AssignRoleAsync(payload));
    }
}