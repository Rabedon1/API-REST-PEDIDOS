namespace Pedidos.Domain.Entities;

public class PedidoDetalle
{
    public int Id { get; set; }
    public int PedidoCabeceraId { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public decimal Subtotal { get; set; }

    public PedidoCabecera PedidoCabecera { get; set; } = null!;
}
