using EducationalPractice.Models;
using EducationalPractice.Support;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtHelper _jwtHelper;

    public AuthController(IConfiguration config, AppDbContext context)
    {
        _jwtHelper = new JwtHelper(config);
        _context = context;
    }

    [HttpPost]
    public async Task<AuthResponse> Authrorize(AuthRequest authRequest)
    {
        List<Employee> employees = await _context.Employees.Include(p => p.Role).ToListAsync();
        var employee = employees.FirstOrDefault(item =>
            item.Login == authRequest.Login && BCrypt.Net.BCrypt.Verify(authRequest.Password, item.Password));
        if (employee == null)
            throw new UnauthorizedAccessException();
        var (accessToken, refreshToken) = _jwtHelper.CreateToken((int)employee.IdEmployee!, employee.Role!.RoleName);
        return new AuthResponse { Token = accessToken, RefreshToken = refreshToken };
    }

    [HttpPost("/refresh")]
    public AuthResponse RefreshToken(string token)
    {
        var principal = _jwtHelper.ValidateToken(token);
        if (principal == null)
            throw new UnauthorizedAccessException();
        var id = principal.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        var role = principal.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
        if (string.IsNullOrWhiteSpace(id) && string.IsNullOrWhiteSpace(role))
            throw new UnauthorizedAccessException();
        var (accessToken, refreshToken) = _jwtHelper.CreateToken(int.Parse(id!), role!);
        return new AuthResponse { Token = accessToken, RefreshToken = refreshToken };
    }
}