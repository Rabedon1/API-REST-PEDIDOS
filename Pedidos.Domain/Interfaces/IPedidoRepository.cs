using Pedidos.Domain.Entities;

namespace Pedidos.Domain.Interfaces;

public interface IPedidoRepository
{
    Task<PedidoCabecera> CrearPedidoAsync(PedidoCabecera pedido);
    Task RegistrarLogAsync(LogAuditoria log);
}
