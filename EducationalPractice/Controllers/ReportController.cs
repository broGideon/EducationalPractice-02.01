using EducationalPractice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Controllers;

[ApiController]
[Route("[controller]")]
public class ReportController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Supervisor")]
    [HttpGet]
    public async Task<List<Report>> GetAll()
    {
        var idStr = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
        if (string.IsNullOrEmpty(idStr))
            throw new UnauthorizedAccessException();
        var id = int.Parse(idStr);
        var listReport = await _context.Reports.Where(r => r.EmployeeId == id).ToListAsync();
        return listReport;
    }

    [Authorize(Roles = "Supervisor")]
    [HttpPost]
    public async Task<Report> Post(Report report)
    {
        report.IdReport = null;
        report.EmployeeId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value!);
        _context.Reports.Add(report);
        await _context.SaveChangesAsync();
        return report;
    }

    [Authorize(Roles = "Supervisor")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var report = await _context.Reports.FirstOrDefaultAsync(r => r.IdReport == id);
        if (report == null)
            return NotFound();
        _context.Reports.Remove(report);
        await _context.SaveChangesAsync();
        return Ok();
    }
}