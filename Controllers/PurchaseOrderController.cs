using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrderController : ControllerBase
{
    private readonly IPurchaseOrderService _purchaseOrderService;

    public PurchaseOrderController(IPurchaseOrderService purchaseOrderService)
    {
        _purchaseOrderService = purchaseOrderService;
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll()
    {
        var purchaseOrders = await _purchaseOrderService.GetAllAsync();
        return Ok(purchaseOrders);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var purchaseOrder = await _purchaseOrderService.GetByIdAsync(id);
        return purchaseOrder is null ? NotFound() : Ok(purchaseOrder);
    }
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] PurchaseOrderCreateDto dto)
    {
        var purchaseOrder = await _purchaseOrderService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = purchaseOrder.Id }, purchaseOrder);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PurchaseOrderUpdateDto dto)
    {
        var purchaseOrder = await _purchaseOrderService.UpdateAsync(id, dto);
        return purchaseOrder is null ? NotFound() : Ok(purchaseOrder);
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _purchaseOrderService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}
