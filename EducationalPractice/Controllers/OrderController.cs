using EducationalPractice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationalPractice.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrderController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<List<Order>> GetAll()
    {
        var listOrder = await _context.Orders.Include(p => p.Client).ToListAsync();
        return listOrder;
    }
    
    [Authorize(Roles = "Supervisor")]
    [HttpGet("report")]
    public async Task<List<Order>> GetInReport()
    {
        DateOnly prevMonth = DateOnly.FromDateTime(DateTime.Today.AddMonths(-1));
        var listOrder = await _context.Orders.Where(o => o.SendDate >= prevMonth).Include(p => p.Client).ToListAsync();
        return listOrder;
    }

    [Authorize]
    [HttpPost]
    public async Task<Order> Post(Order order)
    {
        order.IdOrder = null;
        order.Client = null;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Order order)
    {
        order.Client = null;
        var orderOld = await _context.Orders.FirstOrDefaultAsync(p => p.IdOrder == id);
        if (orderOld == null)
            return NotFound();
        _context.Entry(orderOld).CurrentValues.SetValues(order);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(p => p.IdOrder == id);
        if (order == null)
            return NotFound();
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return Ok();
    }
}