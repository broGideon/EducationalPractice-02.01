using EducationalPractice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmployeeController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Supervisor")]
    [HttpGet]
    public async Task<List<Employee>> GetAll()
    {
        var listEmployee = await _context.Employees.Include(p => p.Role).ToListAsync();
        return listEmployee;
    }

    [Authorize(Roles = "Supervisor")]
    [HttpPost]
    public async Task<Employee> Post(Employee employee)
    {
        employee.IdEmployee = null;
        employee.Role = null;
        employee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    [Authorize(Roles = "Supervisor")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Employee employee)
    {
        employee.Role = null;
        var employeeOld = await _context.Employees.FirstOrDefaultAsync(p => p.IdEmployee == id);
        if (employeeOld == null)
            return NotFound();
        _context.Entry(employeeOld).CurrentValues.SetValues(employee);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize(Roles = "Supervisor")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(p => p.IdEmployee == id);
        if (employee == null)
            return NotFound();
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return Ok();
    }
}