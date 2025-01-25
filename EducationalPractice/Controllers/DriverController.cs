using EducationalPractice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Controllers;

[ApiController]
[Route("[controller]")]
public class DriverController : ControllerBase
{
    private readonly AppDbContext _context;

    public DriverController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<List<Driver>> GetAll()
    {
        var listDriver = await _context.Drivers.ToListAsync();
        return listDriver;
    }

    [Authorize(Roles = "Administrator, Supervisor")]
    [HttpPost]
    public async Task<Driver> Post(Driver driver)
    {
        driver.IdDriver = null;
        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync();
        return driver;
    }

    [Authorize(Roles = "Administrator, Supervisor")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Driver driver)
    {
        var driverOld = await _context.Drivers.FirstOrDefaultAsync(p => p.IdDriver == id);
        if (driverOld == null)
            return NotFound();
        _context.Entry(driverOld).CurrentValues.SetValues(driver);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize(Roles = "Administrator, Supervisor")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var driver = await _context.Drivers.FirstOrDefaultAsync(p => p.IdDriver == id);
        if (driver == null)
            return NotFound();
        _context.Drivers.Remove(driver);
        await _context.SaveChangesAsync();
        return Ok();
    }
}