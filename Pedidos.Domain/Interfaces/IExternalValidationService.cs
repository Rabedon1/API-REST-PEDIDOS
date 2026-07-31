using Pedidos.Domain.Entities;

namespace Pedidos.Domain.Interfaces;

public interface IExternalValidationService
{
    Task<bool> ValidarPedidoAsync(PedidoCabecera pedido);
}
