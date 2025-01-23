using EducationalPractice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController: ControllerBase
{
    private readonly AppDbContext _context;
    
    public OrderController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<List<Order>> GetAll()
    {
        var listOrder = await _context.Orders.Include(p => p.Client).ToListAsync();
        return listOrder;
    }
    
    [HttpGet("{id}")]
    public async Task<Order?> GetById(int id)
    {
        var order = await _context.Orders.Include(p => p.Client).FirstOrDefaultAsync(p => p.IdOrder == id);
        return order;
    }

    [HttpPost]
    public async Task<Order> Post(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Order order)
    {
        var orderOld = await _context.Orders.FirstOrDefaultAsync(p => p.IdOrder == id);
        if (orderOld == null)
            return NotFound();
        _context.Entry(orderOld).CurrentValues.SetValues(order);
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