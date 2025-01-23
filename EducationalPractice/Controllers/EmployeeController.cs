using EducationalPractice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController: ControllerBase
{
    private readonly AppDbContext _context;
    
    public EmployeeController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<List<Employee>> GetAll()
    {
        var listEmployee = await _context.Employees.Include(p => p.Role).ToListAsync();
        return listEmployee;
    }
    
    [HttpGet("{id}")]
    public async Task<Employee?> GetById(int id)
    {
        var employee = await _context.Employees.Include(p => p.Role).FirstOrDefaultAsync(p => p.IdEmployee == id);
        return employee;
    }

    [HttpPost]
    public async Task<Employee> Post(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Employee employee)
    {
        var employeeOld = await _context.Employees.FirstOrDefaultAsync(p => p.IdEmployee == id);
        if (employeeOld == null)
            return NotFound();
        _context.Entry(employeeOld).CurrentValues.SetValues(employee);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(p => p.IdEmployee == id);
        if (employee == null)
            return NotFound();
        _context.Employees.Remove(employee);
        return Ok();
    }
}