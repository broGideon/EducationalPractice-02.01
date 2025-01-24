using EducationalPractice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController: ControllerBase
{
    private readonly AppDbContext _context;
    
    public ClientController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<List<Client>> GetAll()
    {
        var listClient = await _context.Clients.ToListAsync();
        return listClient;
    }
    
    [HttpGet("{id}")]
    public async Task<Client?> GetById(int id)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(p => p.IdClient == id);
        return client;
    }

    [HttpPost]
    public async Task<Client> Post(Client client)
    {
        client.IdClient = null;
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Client client)
    {
        var clientOld = await _context.Clients.FirstOrDefaultAsync(p => p.IdClient == id);
        if (clientOld == null)
            return NotFound();
        _context.Entry(clientOld).CurrentValues.SetValues(client);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(p => p.IdClient == id);
        if (client == null)
            return NotFound();
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return Ok();
    }
}