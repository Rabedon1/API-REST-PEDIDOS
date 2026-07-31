using Pedidos.Application.DTOs;

namespace Pedidos.Application.Interfaces;

public interface IPedidoService
{
    Task<PedidoResponseDto> RegistrarPedidoAsync(PedidoRequestDto request);
}
