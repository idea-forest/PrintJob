using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectLoc.Dtos.Auth;
using ProjectLoc.Dtos.Auth.Request;
using ProjectLoc.Dtos.Auth.Response;
using ProjectLoc.Services;
using ProjectLoc.Data;
using ProjectLoc.Models;

namespace ProjectLoc.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // Identity package
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly ApiDbContext _context;

    public AuthController(ApiDbContext dbContext, UserManager<ApplicationUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _context = dbContext;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDTO user)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser != null)
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = new List<string>() { "Email already Registered" },
                    Success = false
                });
            }

            Team? team = await _context.Teams.FirstOrDefaultAsync(t => t.UserName == user.UserName);
            if (team != null)
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = new List<string>() { "team already exists" },
                    Success = false
                });
            }

            Team newTeam = new Team()
            {
                UserName = user.UserName,
            };
            await _context.Teams.AddAsync(newTeam);
            await _context.SaveChangesAsync();

            ApplicationUser newUser = new ApplicationUser()
            {
                Email = user.Email,
                UserName = user.UserName,
                TeamId = newTeam.Id
            };

            IdentityResult? created = await _userManager.CreateAsync(newUser, user.Password);
            if (created.Succeeded)
            {
                AuthResult authResult = await _jwtService.GenerateToken(newUser);
                //return a token
                return Ok(authResult);
            }
            else
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = created.Errors.Select(e => e.Description).ToList(),
                    Success = false
                });
            }
        }

        return BadRequest(new RegisterResponseDTO()
        {
            Errors = new List<string>() { "Invalid payload" },
            Success = false
        });
    }

    [HttpPost("google")]
    public async Task<IActionResult> LoginByEmail(GoogleUserDTO user)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser == null)
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = new List<string>() { "Email address is not registered." },
                    Success = false
                });
            }

            // If the user exists, generate a token without password verification
            AuthResult authResult = await _jwtService.GenerateToken(existingUser);

            // Return a token (or any necessary data)
            return Ok(authResult);
        }

        return BadRequest(new RegisterResponseDTO()
        {
            Errors = new List<string>() { "Invalid payload" },
            Success = false
        });
    }


    [HttpGet("{TeamName}")]
    public async Task<IActionResult> checkTeamName(string TeamName)
    {
        Team? team = await _context.Teams.FirstOrDefaultAsync(t => t.UserName == TeamName);

        if (team != null) // Check if team is not null
        {
            return Ok(team);
        }

        return NotFound();
    }

    [HttpPost("login")]
    public async Task<IActionResult> login(LoginUserDTO user)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser == null)
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = new List<string>() { "Email address is not registered." },
                    Success = false
                });
            }

            bool isUserCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);
            if (isUserCorrect)
            {
                AuthResult authResult = await _jwtService.GenerateToken(existingUser);
                //return a token
                return Ok(authResult);
            }
            else
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = new List<string>() { "Wrong password" },
                    Success = false
                });
            }
        }

        return BadRequest(new RegisterResponseDTO()
        {
            Errors = new List<string>() { "Invalid payload" },
            Success = false
        });
    }

    [HttpPost("loginByClientApp")]
    public async Task<IActionResult> LoginByClientApp(LoginUserDTOApp user)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser == null)
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = new List<string>() { "Email address is not registered." },
                    Success = false
                });
            }

            Team? team = await _context.Teams.FirstOrDefaultAsync(t => t.UserName == user.UserName);

            //if (team != null || existingUser.TeamId != team?.Id)
            //{
            //    return BadRequest(new RegisterResponseDTO()
            //    {
            //        Errors = new List<string>() { "Invalid team or user not associated with the team." },
            //        Success = false
            //    });
            //}

            bool isUserCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);
            if (isUserCorrect)
            {
                AuthResult authResult = await _jwtService.GenerateToken(existingUser);
                AuthResult response = new AuthResult()
                {
                    Success = true,
                    Token = authResult.Token,
                    User = existingUser
                };
                return Ok(response);
            }
            else
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = new List<string>() { "Wrong password" },
                    Success = false
                });
            }
        }

        return BadRequest(new RegisterResponseDTO()
        {
            Errors = new List<string>() { "Invalid payload" },
            Success = false
        });
    }

    [HttpPost("refreshtoken")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDTO tokenRequest)
    {
        if (ModelState.IsValid)
        {
            var verified = await _jwtService.VerifyToken(tokenRequest);
            //
            if (!verified.Success)
            {
                return BadRequest(new AuthResult()
                {
                    // Errors = new List<string> { "invalid Token" },
                    Errors = verified.Errors,
                    Success = false
                });
            }

            var tokenUser = await _userManager.FindByIdAsync(verified.UserId);
            AuthResult authResult = await _jwtService.GenerateToken(tokenUser);
            //return a token
            return Ok(authResult);


        }

        return BadRequest(new AuthResult()
        {
            Errors = new List<string> { "invalid Payload" },
            Success = false
        });
    }
}