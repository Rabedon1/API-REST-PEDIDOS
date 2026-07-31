namespace Pedidos.Application.DTOs;

public class PedidoResponseDto
{
    public int PedidoId { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
}
