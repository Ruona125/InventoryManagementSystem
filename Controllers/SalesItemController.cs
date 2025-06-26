using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesItemController : ControllerBase
{
    private readonly ISalesItemService _salesItemService;
    public SalesItemController(ISalesItemService salesItemService)
    {
        _salesItemService = salesItemService;
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll()
    {
        var salesItems = await _salesItemService.GetAllAsync();
        return Ok(salesItems);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var salesItem = await _salesItemService.GetByIdAsync(id);
        return salesItem is null ? NotFound() : Ok(salesItem);
    }
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] SaleItemCreateDto dto)
    {
        var salesItem = await _salesItemService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = salesItem.Id }, salesItem);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SaleItemUpdateDto dto)
    {
        var salesItem = await _salesItemService.UpdateAsync(id, dto);
        return salesItem is null ? NotFound() : Ok(salesItem);
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _salesItemService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}