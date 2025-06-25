using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISalesService _salesService;

    public SalesController(ISalesService salesService)
    {
        _salesService = salesService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll()
    {
        var sales = await _salesService.GetAllAsync();
        return Ok(sales);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var sale = await _salesService.GetByIdAsync(id);
        return sale is null ? NotFound() : Ok(sale);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] SalesCreateDto dto)
    {
        var sale = await _salesService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(Guid id, SalesUpdateDto dto)
    {
        var sale = await _salesService.UpdateAsync(id, dto);
        return sale is null ? NotFound() : Ok(sale);
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _salesService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}