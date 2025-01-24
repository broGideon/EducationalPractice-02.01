using EducationalPractice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportController: ControllerBase
{
    private readonly AppDbContext _context;
    
    public ReportController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<List<Report>> GetAll()
    {
        var listReport = await _context.Reports.ToListAsync();
        return listReport;
    }
    
    [HttpGet("{id}")]
    public async Task<Report?> GetById(int id)
    {
        var report = await _context.Reports.FirstOrDefaultAsync(p => p.IdReport == id);
        return report;
    }

    [HttpPost]
    public async Task<Report> Post(Report report)
    {
        report.IdReport = null;
        _context.Reports.Add(report);
        await _context.SaveChangesAsync();
        return report;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Report report)
    {
        var reportOld = await _context.Reports.FirstOrDefaultAsync(p => p.IdReport == id);
        if (reportOld == null)
            return NotFound();
        _context.Entry(reportOld).CurrentValues.SetValues(report);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var report = await _context.Reports.FirstOrDefaultAsync(p => p.IdReport == id);
        if (report == null)
            return NotFound();
        _context.Reports.Remove(report);
        await _context.SaveChangesAsync();
        return Ok();
    }
}