using Microsoft.AspNetCore.Mvc;
using Pedidos.Application.DTOs;
using Pedidos.Application.Interfaces;

namespace Pedidos.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidoService;
    private readonly Serilog.ILogger _logger;

    public PedidosController(IPedidoService pedidoService, Serilog.ILogger logger)
    {
        _pedidoService = pedidoService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> RegistrarPedido([FromBody] PedidoRequestDto request)
    {
        _logger.Information("Iniciando registro de pedido para el cliente {ClienteId}", request.ClienteId);
        
        var response = await _pedidoService.RegistrarPedidoAsync(request);
        
        _logger.Information("Pedido {PedidoId} registrado exitosamente", response.PedidoId);
        
        return CreatedAtAction(nameof(RegistrarPedido), new { id = response.PedidoId }, response);
    }
}
