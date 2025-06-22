using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WareHouseController : ControllerBase
{
    private readonly IWareHouseService _wareHouseService;
    public WareHouseController(IWareHouseService wareHouseService)
    {
        _wareHouseService = wareHouseService;
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll()
    {
        var wareHouses = await _wareHouseService.GetAllAsync();
        return Ok(wareHouses);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var wareHouse = await _wareHouseService.GetByIdAsync(id);
        return wareHouse is null ? NotFound() : Ok(wareHouse);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] WareHouseCreateDto dto)
    {
        var wareHouse = await _wareHouseService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = wareHouse.Id }, wareHouse);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(Guid id, WareHouseUpdateDto dto)
    {
        var wareHouse = await _wareHouseService.UpdateAsync(id, dto);
        return wareHouse is null ? NotFound() : Ok(wareHouse);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _wareHouseService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}