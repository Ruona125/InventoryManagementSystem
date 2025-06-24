using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebAPI.DTOs;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductStockController : ControllerBase
{
    private readonly IProductStockService _productStockService;

    public ProductStockController(IProductStockService productStockService)
    {
        _productStockService = productStockService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await _productStockService.GetAllAsync();
        return Ok(stocks);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var stock = await _productStockService.GetByIdAsync(id);
        return stock is null ? NotFound() : Ok(stock);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] ProductStockCreateDto dto)
    {
        var stock = await _productStockService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stock);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProductStockUpdateDto dto)
    {
        var stock = await _productStockService.UpdateAsync(id, dto);
        return stock is null ? NotFound() : Ok(stock);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _productStockService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}