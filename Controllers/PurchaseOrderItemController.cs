using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrderItemController : ControllerBase
{
    private readonly IPurchaseOrderItemService _purchaseOrderItemService;

    public PurchaseOrderItemController(IPurchaseOrderItemService purchaseOrderItemService)
    {
        _purchaseOrderItemService = purchaseOrderItemService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll()
    {
        var purchaseOrderItems = await _purchaseOrderItemService.GetAllAsync();
        return Ok(purchaseOrderItems);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var purchaseOrderItem = await _purchaseOrderItemService.GetByIdAsync(id);
        return purchaseOrderItem is null ? NotFound() : Ok(purchaseOrderItem);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] PurchaseOrderItemCreateDto dto)
    {
        var purchaseOrderItem = await _purchaseOrderItemService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = purchaseOrderItem.Id }, purchaseOrderItem);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PurchaseOrderItemUpdateDto dto)
    {
        var purchaseOrderItem = await _purchaseOrderItemService.UpdateAsync(id, dto);
        return purchaseOrderItem is null ? NotFound() : Ok(purchaseOrderItem);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _purchaseOrderItemService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}