using Pedidos.Domain.Entities;
using Pedidos.Domain.Interfaces;

namespace Pedidos.Infrastructure.Services;

public class ExternalValidationService : IExternalValidationService
{
    public async Task<bool> ValidarPedidoAsync(PedidoCabecera pedido)
    {
        // Simula la llamada a un servicio externo (ej. HTTP Client)
        // Agregamos un delay simulado
        await Task.Delay(500);

        // Simularemos que el pedido es válido si el total es mayor a 0
        if (pedido.Total <= 0)
        {
            return false;
        }

        return true; // El pedido es válido según el servicio externo
    }
}
