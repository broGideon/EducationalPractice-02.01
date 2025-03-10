using EducationalPractice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Controllers;

[ApiController]
[Route("[controller]")]
public class RoleController : ControllerBase
{
    private readonly AppDbContext _context;

    public RoleController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Supervisor")]
    [HttpGet]
    public async Task<List<Role>> GetAll()
    {
        var listRole = await _context.Roles.ToListAsync();
        return listRole;
    }

    [Authorize(Roles = "Supervisor")]
    [HttpPost]
    public async Task<Role> Post(Role role)
    {
        role.IdRole = null;
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();
        return role;
    }

    [Authorize(Roles = "Supervisor")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Role role)
    {
        var roleOld = await _context.Roles.FirstOrDefaultAsync(p => p.IdRole == id);
        if (roleOld == null)
            return NotFound();
        _context.Entry(roleOld).CurrentValues.SetValues(role);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize(Roles = "Supervisor")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var role = await _context.Roles.FirstOrDefaultAsync(p => p.IdRole == id);
        if (role == null)
            return NotFound();
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();
        return Ok();
    }
}