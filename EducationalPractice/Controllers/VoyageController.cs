using EducationalPractice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Controllers;

[ApiController]
[Route("[controller]")]
public class VoyageController: ControllerBase 
{
    private readonly AppDbContext _context;
    
    public VoyageController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<List<Voyage>> GetAll()
    {
        var listVoyage = await _context.Voyages.Include(p => p.Driver).Include(p => p.Driver).Include(p => p.Order).ThenInclude(o => o!.Client).ToListAsync();
        return listVoyage;
    }
    
    [HttpGet("{id}")]
    public async Task<Voyage?> GetById(int id)
    {
        var voyage = await _context.Voyages.Include(p => p.Driver).Include(p => p.Driver).Include(p => p.Order).ThenInclude(o => o!.Client).FirstOrDefaultAsync(p => p.IdVoyage == id);
        return voyage;
    }

    [HttpPost]
    public async Task<Voyage> Post(Voyage voyage)
    {
        voyage.IdVoyage = null;
        _context.Voyages.Add(voyage);
        await _context.SaveChangesAsync();
        return voyage;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Voyage voyage)
    {
        var voyageOld = await _context.Voyages.FirstOrDefaultAsync(p => p.IdVoyage == id);
        if (voyageOld == null)
            return NotFound();
        _context.Entry(voyageOld).CurrentValues.SetValues(voyage);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var voyage = await _context.Voyages.FirstOrDefaultAsync(p => p.IdVoyage == id);
        if (voyage == null)
            return NotFound();
        _context.Voyages.Remove(voyage);
        return Ok();
    }
}