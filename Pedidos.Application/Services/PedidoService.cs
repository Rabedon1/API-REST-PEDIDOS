using Pedidos.Application.DTOs;
using Pedidos.Application.Interfaces;
using Pedidos.Domain.Entities;
using Pedidos.Domain.Interfaces;

namespace Pedidos.Application.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IExternalValidationService _validationService;

    public PedidoService(IPedidoRepository pedidoRepository, IExternalValidationService validationService)
    {
        _pedidoRepository = pedidoRepository;
        _validationService = validationService;
    }

    public async Task<PedidoResponseDto> RegistrarPedidoAsync(PedidoRequestDto request)
    {
        // 1. Validaciones básicas
        if (request.Items == null || !request.Items.Any())
            throw new ArgumentException("El pedido debe contener al menos un producto.");

        // 2. Mapeo Manual y Cálculo de Totales
        var nuevoPedido = new PedidoCabecera
        {
            ClienteId = request.ClienteId,
            Usuario = request.Usuario,
            Fecha = DateTime.Now,
            Estado = "Registrado",
            Detalles = new List<PedidoDetalle>()
        };

        decimal totalPedido = 0;
        foreach (var item in request.Items)
        {
            var subtotal = item.Cantidad * item.Precio;
            totalPedido += subtotal;

            nuevoPedido.Detalles.Add(new PedidoDetalle
            {
                ProductoId = item.ProductoId,
                Cantidad = item.Cantidad,
                Precio = item.Precio,
                Subtotal = subtotal
            });
        }
        nuevoPedido.Total = totalPedido;

        // 3. Validación externa
        var isValid = await _validationService.ValidarPedidoAsync(nuevoPedido);
        if (!isValid)
        {
            nuevoPedido.Estado = "Rechazado";
            await RegistrarLogAsync(nuevoPedido, "Validación Fallida", "El servicio externo rechazó el pedido.", "Warning");
            throw new InvalidOperationException("La validación externa del pedido falló.");
        }

        // 4. Iniciar Transacción abstracta
        await _pedidoRepository.BeginTransactionAsync();
        try
        {
            // Guardar pedido
            await _pedidoRepository.CrearPedidoAsync(nuevoPedido);

            // Guardar log
            await RegistrarLogAsync(nuevoPedido, "Pedido Registrado", $"Pedido {nuevoPedido.Id} registrado exitosamente para el cliente {nuevoPedido.ClienteId}", "Info");

            // Confirmar transacción
            await _pedidoRepository.CommitTransactionAsync();

            return new PedidoResponseDto
            {
                PedidoId = nuevoPedido.Id,
                Total = nuevoPedido.Total,
                Estado = nuevoPedido.Estado,
                Mensaje = "Pedido registrado correctamente."
            };
        }
        catch (Exception ex)
        {
            // Revertir transacción en caso de error
            await _pedidoRepository.RollbackTransactionAsync();

            // Intentamos loguear el error fuera de la transacción revertida
            await RegistrarLogAsync(nuevoPedido, "Error al Registrar", ex.Message, "Error");
            throw;
        }
    }

    private async Task RegistrarLogAsync(PedidoCabecera pedido, string evento, string descripcion, string nivel)
    {
        var log = new LogAuditoria
        {
            Evento = evento,
            Descripcion = descripcion,
            Nivel = nivel,
            Usuario = pedido.Usuario,
            Fecha = DateTime.Now
        };
        await _pedidoRepository.RegistrarLogAsync(log);
    }
}
