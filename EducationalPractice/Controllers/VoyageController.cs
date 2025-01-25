using EducationalPractice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Controllers;

[ApiController]
[Route("[controller]")]
public class VoyageController : ControllerBase
{
    private readonly AppDbContext _context;

    public VoyageController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Logistician, Supervisor")]
    [HttpGet]
    public async Task<List<Voyage>> GetAll()
    {
        var listVoyage = await _context.Voyages.Include(p => p.Transport).Include(p => p.Driver).Include(p => p.Order)
            .ThenInclude(o => o!.Client).ToListAsync();
        return listVoyage;
    }

    [Authorize(Roles = "Logistician, Supervisor")]
    [HttpPost]
    public async Task<Voyage> Post(Voyage voyage)
    {
        voyage.IdVoyage = null;
        voyage.Driver = null;
        voyage.Transport = null;
        voyage.Order = null;
        _context.Voyages.Add(voyage);
        await _context.SaveChangesAsync();
        return voyage;
    }

    [Authorize(Roles = "Logistician, Supervisor")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Voyage voyage)
    {
        //TODO При обновление статуса, статус автомобиля и заказа тоже обновляются, а также обновляется время в заказе на текущее
        voyage.Driver = null;
        voyage.Transport = null;
        voyage.Order = null;
        var voyageOld = await _context.Voyages.FirstOrDefaultAsync(p => p.IdVoyage == id);
        if (voyageOld == null)
            return NotFound();
        _context.Entry(voyageOld).CurrentValues.SetValues(voyage);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize(Roles = "Logistician, Supervisor")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var voyage = await _context.Voyages.FirstOrDefaultAsync(p => p.IdVoyage == id);
        if (voyage == null)
            return NotFound();
        _context.Voyages.Remove(voyage);
        await _context.SaveChangesAsync();
        return Ok();
    }
}