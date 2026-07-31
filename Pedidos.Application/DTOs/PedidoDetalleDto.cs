namespace Pedidos.Application.DTOs;

public class PedidoDetalleDto
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
}
