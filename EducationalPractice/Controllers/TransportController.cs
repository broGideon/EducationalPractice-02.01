using EducationalPractice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Controllers;

[ApiController]
[Route("[controller]")]
public class TransportController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransportController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<List<Transport>> GetAll()
    {
        var listTransport = await _context.Transports.Include(p => p.Status).ToListAsync();
        return listTransport;
    }

    [HttpGet("{id}")]
    public async Task<Transport?> GetById(int id)
    {
        var transport = await _context.Transports.Include(p => p.Status).FirstOrDefaultAsync(p => p.IdTransport == id);
        return transport;
    }

    [HttpPost]
    public async Task<Transport> Post(Transport transport)
    {
        transport.IdTransport = null;
        transport.Status = null;
        _context.Transports.Add(transport);
        await _context.SaveChangesAsync();
        return transport;
    }

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