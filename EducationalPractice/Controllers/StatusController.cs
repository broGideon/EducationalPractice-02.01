using EducationalPractice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusController: ControllerBase
{
    private readonly AppDbContext _context;
    
    public StatusController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<List<Status>> GetAll()
    {
        var listStatus = await _context.Status.ToListAsync();
        return listStatus;
    }
    
    [HttpGet("{id}")]
    public async Task<Status?> GetById(int id)
    {
        var status = await _context.Status.FirstOrDefaultAsync(p => p.IdStatus == id);
        return status;
    }

    [HttpPost]
    public async Task<Status> Post(Status status)
    {
        _context.Status.Add(status);
        await _context.SaveChangesAsync();
        return status;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Status status)
    {
        var statusOld = await _context.Status.FirstOrDefaultAsync(p => p.IdStatus == id);
        if (statusOld == null)
            return NotFound();
        _context.Entry(statusOld).CurrentValues.SetValues(status);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var status = await _context.Status.FirstOrDefaultAsync(p => p.IdStatus == id);
        if (status == null)
            return NotFound();
        _context.Status.Remove(status);
        return Ok();
    }
}