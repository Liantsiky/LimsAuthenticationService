using Microsoft.AspNetCore.Mvc; // For ControllerBase and attributes like [ApiController]
using Microsoft.EntityFrameworkCore;

using LimsAuthenticationService.Models;
using LimsAuthenticationService.Data;
using LimsAuthenticationService.Services;
using LimsAuthenticationService.Dto;
using LimsAuthenticationService.Utils; // For ApiResponse

namespace LimsAuthenticationService.Controllers;
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly AuthDbContext _context;

    public AuthController(IAuthService authService, AuthDbContext context)
    {
        _authService = authService;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto userDto)
    {
         if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse
            {
                Data = null,
                ViewBag = null,
                IsSuccess = false,
                Message = "Invalid model state.",
                StatusCode = 400
            });
        }
        if (string.IsNullOrWhiteSpace(userDto.Identifiant))
        {
            return BadRequest(new ApiResponse
            {
                Data = null,
                ViewBag = null,
                IsSuccess = false,
                Message = "Identifiant cannot be null or empty.",
                StatusCode = 400
            });
        }
        try
        {
            await _authService.RegisterUserAsync(userDto);
            // Utilisateur user = await _context.Utilisateurs.FirstAsync(u => u.Identifiant == userDto.Identifiant);
            
            Utilisateur user = await _context.Utilisateurs
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstAsync(u => u.Identifiant == userDto.Identifiant);

            // Map to the response DTO
            RegisterDto responseDto = new RegisterDto
            {
                Identifiant = user.Identifiant,
                Roles = user.UserRoles.Select(ur => ur.Role.Designation).ToList(),
                Password = user.Password
            };
            return Ok(new ApiResponse
            {
                Data = responseDto,
                ViewBag = null,
                IsSuccess = true,
                Message = "Datas retrieved successfully.",
                StatusCode = 200
            });
        }
        catch (InvalidOperationException)
        {
            return NotFound(new ApiResponse
            {
                Data = null,
                ViewBag = null,
                IsSuccess = false,
                Message = "User not found after registration.",
                StatusCode = 404
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse
            {
                Data = null,
                ViewBag = null,
                IsSuccess = false,
                Message = $"An error occurred: {ex.Message}",
                StatusCode = 500
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _authService.LoginUserAsync(loginDto);
        if (token == "Invalid credentials")
        {
            return Unauthorized(new ApiResponse
            {
                Data = null,
                ViewBag = null,
                IsSuccess = false,
                Message = "Invalid username or password.",
                StatusCode = 500
            });
        }

        return Ok(new ApiResponse
            {
                Data = token,
                ViewBag = null,
                IsSuccess = true,
                Message = "Token created successfully",
                StatusCode = 200
            });
    }
}


