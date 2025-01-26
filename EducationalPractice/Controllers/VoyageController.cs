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
        var listVoyage = await _context.Voyages
            .Include(p => p.Transport)
            .Include(p => p.Driver)
            .Include(p => p.Order)
            .ThenInclude(o => o!.Client)
            .ToListAsync();
        return listVoyage;
    }

    [Authorize(Roles = "Supervisor")]
    [HttpGet("report")]
    public async Task<List<Voyage>> GetInReport()
    {
        var prevMonth = DateOnly.FromDateTime(DateTime.Today.AddMonths(-1));
        var listVoyage = await _context.Voyages.Where(v => v.StartDate >= prevMonth).Include(p => p.Transport)
            .ToListAsync();
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
        voyage.Driver = null;
        voyage.Transport = null;
        voyage.Order = null;
        var voyageOld = await _context.Voyages.FirstOrDefaultAsync(p => p.IdVoyage == id);
        if (voyageOld == null)
            return NotFound();
        _context.Entry(voyageOld).CurrentValues.SetValues(voyage);
        if (voyage.Status == "Выполнено")
        {
            var oldOrder = await _context.Orders.FirstOrDefaultAsync(o => o.IdOrder == voyage.OrderId);
            if (oldOrder != null)
            {
                oldOrder.Status = voyage.Status;
                oldOrder.ArriveDate = DateOnly.FromDateTime(DateTime.Today);
                _context.Entry(oldOrder).CurrentValues.SetValues(oldOrder);
            }

            var oldTransport = await _context.Transports.FirstOrDefaultAsync(t => t.IdTransport == voyage.TransportId);
            if (oldTransport != null)
            {
                oldTransport.StatusId = 1;
                _context.Entry(oldTransport).CurrentValues.SetValues(oldTransport);
            }
        }

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