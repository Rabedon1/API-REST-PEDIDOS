namespace Pedidos.Application.DTOs;

public class PedidoRequestDto
{
    public int ClienteId { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public List<PedidoDetalleDto> Items { get; set; } = new();
}
