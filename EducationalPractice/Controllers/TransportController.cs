using EducationalPractice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TransportController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransportController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<List<Transport>> GetAll()
    {
        var listTransport = await _context.Transports.Include(p => p.Status).ToListAsync();
        return listTransport;
    }

    [Authorize(Roles = "Administrator, Supervisor")]
    [HttpPost]
    public async Task<Transport> Post(Transport transport)
    {
        transport.IdTransport = null;
        transport.Status = null;
        _context.Transports.Add(transport);
        await _context.SaveChangesAsync();
        return transport;
    }

    [Authorize(Roles = "Administrator, Supervisor")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Transport transport)
    {
        transport.Status = null;
        var transportOld = await _context.Transports.FirstOrDefaultAsync(p => p.IdTransport == id);
        if (transportOld == null)
            return NotFound();
        _context.Entry(transportOld).CurrentValues.SetValues(transport);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize(Roles = "Administrator, Supervisor")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var transport = await _context.Transports.FirstOrDefaultAsync(p => p.IdTransport == id);
        if (transport == null)
            return NotFound();
        _context.Transports.Remove(transport);
        await _context.SaveChangesAsync();
        return Ok();
    }
}